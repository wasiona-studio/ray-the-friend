using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using OpenAI;
using TMPro;

public class ChatGptManager : MonoBehaviour
{
    public static ChatGptManager instance;

    public AudioSource dogSource;
    public AwsManager awsManager;
    public string pathToKeys;
    public Keys keys;
    public OpenAIApi OpenAi;

    private void Awake()
    {
        instance = this;
        keys = new Keys(pathToKeys + "/keys.json" /*"C:/keys.json"*/);
        OpenAi = new OpenAIApi(keys.openai_sk, keys.openai_org);
    }


    private List<ChatMessage> _messages = new();

    [TextArea] [SerializeField] private string startingPrompt;
    private bool _logMessages;

    [SerializeField] private TextMeshProUGUI output;
    private string _gptResponse;

    private void Start()
    {
        AskChatGPT(startingPrompt);
    }


    public void TestSpeech(string prompt)
    {
        awsManager.Speak(prompt, dogSource, () =>
        {
            print("Playing sound");
            output.text = _gptResponse;
        });
    }

    public async void AskChatGPT(string text)
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

        if (!_logMessages)
        {
            _logMessages = true;
            return;
        }

        _gptResponse = chatResponse.Content;
        print($"GPT: {_gptResponse}");


        awsManager.Speak(_gptResponse, dogSource, () =>
        {
            print("Playing sound");
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