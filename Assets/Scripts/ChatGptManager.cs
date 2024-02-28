using System;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using TMPro;

public class ChatGptManager : MonoBehaviour
{
    public static ChatGptManager instance;

    public string voice;
    
    public AudioSource dogSource;
    public AudioClip audioClip;
    public AWS_Manager awsManager;
    private void Awake()
    {
        instance = this;
    }

    private OpenAIApi openAi = new OpenAIApi("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","org-hXczioOTdBD74Xyvool9Dhcz");

    private List<ChatMessage> messages = new();

    [TextArea] [SerializeField] private string startingPrompt;
    private bool logMessages;

    [SerializeField] private TextMeshProUGUI output;
    private string GPTresponse;

    private void Start()
    {
        AskChatGPT(startingPrompt);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            awsManager.Speak("GPT response is not available at the moment, please try later", dogSource, () =>
            {
                print("Playing sound");
                output.text = GPTresponse;
            });
        }
    }

    public async void AskChatGPT(string text)
    {
        ChatMessage newMessage = new()
        {
            Content = text,
            Role = "user"
        };

        messages.Add(newMessage);

        CreateChatCompletionRequest request = new CreateChatCompletionRequest();

        request.Messages = messages;
        request.Model = "gpt-3.5-turbo";

        var response = await openAi.CreateChatCompletion(request);

        if (response.Choices == null || response.Choices.Count <= 0) return;

        var chatResponse = response.Choices[0].Message;
        messages.Add(chatResponse);

        if (!logMessages)
        {
            logMessages = true;
            return;
        }

        GPTresponse = chatResponse.Content;
        print($"GPT: {GPTresponse}");

       
        awsManager.Speak(GPTresponse,dogSource, () =>
        {        
            print("Playing sound");
            output.text = GPTresponse;
        });
    }
}