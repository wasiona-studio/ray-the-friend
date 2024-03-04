using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using OpenAI;
using TMPro;

public class ChatGptManager : GenericSingletonClass<ChatGptManager>
{
    public AudioSource dogSource;
    public string pathToKeys;
    public Keys keys;
    public OpenAIApi OpenAi;
    
    private List<ChatMessage> _messages = new();
    private string _gptResponse;
    
    [TextArea] [SerializeField] private string startingPrompt;
    [SerializeField] private TextMeshProUGUI output;

    public static Action<string,bool> OnAskGpt;

    private void OnEnable()
    {
        keys = new Keys(pathToKeys + "/keys.json" /*"C:/keys.json"*/);
        OpenAi = new OpenAIApi(keys.openai_sk, keys.openai_org);
        
        OnAskGpt += AskChatGpt;
    }

    private void OnDisable()
    {
        OnAskGpt -= AskChatGpt;
    }


    private void Start()
    {
        AskChatGpt(startingPrompt,false);
    }

    private async void AskChatGpt(string text,bool speak)
    {
        ChatMessage newMessage = new()
        {
            Content = text,
            Role = "user"
        };

        _messages.Add(newMessage);

        CreateChatCompletionRequest request = new CreateChatCompletionRequest();

        request.Messages = _messages;
        request.Model = "gpt-3.5-turbo";

        var response = await OpenAi.CreateChatCompletion(request);

        if (response.Choices == null || response.Choices.Count <= 0) return;

        var chatResponse = response.Choices[0].Message;
        _messages.Add(chatResponse);

        if (!speak)
            return;
        GetResponse(chatResponse);
    }

    private void GetResponse(ChatMessage chatMessage)
    {
        _gptResponse = chatMessage.Content;
        PlaySpeech(_gptResponse);
        
    }

    public void PlaySpeech(string prompt)
    {
        AwsManager.OnSpeak?.Invoke(prompt, dogSource, () =>
        {
            output.text = _gptResponse;
        });
    }
    
}

[Serializable]
public class Keys
{
    public string aws_sk;
    public string aws_ak;
    public string openai_sk;
    public string openai_org;

    public Keys(string path)
    {
        Debug.Log("Keys got");
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