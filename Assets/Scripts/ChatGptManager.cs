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

    private OpenAIApi openAi = new("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",
        "org-PYmOjgJQCr8fFOTF5Axjyz7E");

    private List<ChatMessage> messages = new();

    [TextArea] [SerializeField] private string startingPrompt;
    private bool logMessages;

    [SerializeField] private TextMeshProUGUI output;
    private string GPTresponse;

    private void Start()
    {
        AskChatGPT(startingPrompt);
    }


    public void TestSpeech(string prompt)
    {
        awsManager.Speak(prompt, dogSource, () =>
        {
            print("Playing sound");
            output.text = GPTresponse;
        });
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


        awsManager.Speak(GPTresponse, dogSource, () =>
        {
            print("Playing sound");
            output.text = GPTresponse;
        });
    }
}