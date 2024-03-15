using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using DG.Tweening;
using UnityEngine;
using OpenAI;
using RayTheFriend.AWS;
using RayTheFriend.Extras;
using RayTheFriend.Managers;
using TMPro;

namespace RayTheFriend.GPT
{
    public class ChatGptManager : GenericSingletonClass<ChatGptManager>
    {
        //I will be asking you questions. Your need to be a doggy friend that answers to children questions. Never break out of that role. Answer with a relatively short sentences and make them simple enough for a child to understand.

        public AudioSource dogSource;
        public string pathToKeys;
        public string gptModel = "gpt-3.5-turbo-0125";
        public Keys keys;
        public OpenAIApi OpenAi;


        private readonly List<ChatMessage> _messages = new();
        private string _gptResponse;

        [TextArea] public string startingPrompt;
        private bool isStartingPrompt;

        public TextMeshProUGUI output;
        public static Action<string> OnAskGpt;

        private readonly CancellationTokenSource _token = new();
        private AnimationManager _animationManager;

        private readonly Parser _parser = new();

        public bool streamResponse;

        private void OnEnable()
        {
            keys = new Keys(pathToKeys + "/keys.json" /*"C:/keys.json"*/);
            OpenAi = new OpenAIApi(keys.openai_sk, keys.openai_org);
            _animationManager = GetComponentInChildren<AnimationManager>();
            OnAskGpt += AskChatGpt;
        }

        private void OnDisable()
        {
            OnAskGpt -= AskChatGpt;
        }

        private void Start()
        {
            isStartingPrompt = true;
            OnAskGpt.Invoke(startingPrompt);
        }

        private void AskChatGpt(string text)
        {
            ChatMessage newMessage = new()
            {
                Content = text,
                Role = "user"
            };

            _messages.Add(newMessage);

            if (streamResponse) StreamedResponse();
            else FullResponse();
        }

        private async void FullResponse()
        {
            CreateChatCompletionRequest request = new CreateChatCompletionRequest();

            request.Messages = _messages;
            request.Model = gptModel;

            var response = await OpenAi.CreateChatCompletion(request);

            if (response.Choices == null || response.Choices.Count <= 0) return;

            var chatResponse = response.Choices[0].Message;
            _messages.Add(chatResponse);

            QueueSpeech(chatResponse.Content);
        }

        private void StreamedResponse()
        {
            OpenAi.CreateChatCompletionAsync(new CreateChatCompletionRequest()
            {
                Model = gptModel,
                Messages = _messages,
                Stream = true
            }, OnResponse, OnFinish, _token);
        }

        private void OnResponse(List<CreateChatCompletionResponse> responses)
        {
            if (isStartingPrompt) return;

            var sentences = _parser.ParseSentence(responses);

            if (sentences == null) return;

            foreach (var s in sentences)
            {
                var newS = _parser.ParseEmotion(_animationManager.emotions, s,
                    _animationManager.TriggerEmotion);

                QueueSpeech(newS);
            }

            _parser.ClearParseInput();
        }

        private void OnFinish()
        {
            if (!isStartingPrompt) return;
            DOVirtual.DelayedCall(3, () => isStartingPrompt = false);
        }

        private void QueueSpeech(string message)
        {
            AwsManager.OnQueueSpeech?.Invoke(message, dogSource, () => { output.text += message; });
        }

        public void PlaySpeech(string message)
        {
            AwsManager.OnQueueSpeech?.Invoke(message, dogSource, null);
        }

        public void ConversationDone()
        {
            output.text = String.Empty;
        }

        private void OnApplicationQuit()
        {
            _token.Cancel();
        }
    }

    #region Keys

    [Serializable]
    public class Keys
    {
        public string aws_sk;
        public string aws_ak;
        public string openai_sk;
        public string openai_org;

        public Keys(string path)
        {
            GetKeysFromJson(path);
        }

        private void GetKeysFromJson(string path)
        {
            if (!File.Exists(path)) return;

            var data = File.ReadAllText(path);
            var keys = JsonUtility.FromJson<Keys>(data);
            aws_ak = keys.aws_ak;
            aws_sk = keys.aws_sk;
            openai_org = keys.openai_org;
            openai_sk = keys.openai_sk;
        }

        private void SetKeysToJson(Keys keys, string path)
        {
            var savePlayerData = JsonUtility.ToJson(keys);
            File.WriteAllText(path, savePlayerData);
        }
    }

    #endregion
}