using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading;
using RayTheFriend.GPT;

namespace OpenAI
{
    public class StreamResponse : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Text text;

        private OpenAIApi openai;
        private CancellationTokenSource token = new CancellationTokenSource();
        private Keys keys;
        private void Start()
        {
            keys = new Keys(/*pathToKeys + "/keys.json"*/ "C:/keys.json");
            openai = new OpenAIApi(keys.openai_sk,keys.openai_org);
            button.onClick.AddListener(SendMessage);
        }
        
        private void SendMessage()
        {
            button.enabled = false;
            var req = new CreateChatCompletionRequest{
                Model = "gpt-3.5-turbo",
                Messages = new List<ChatMessage>
                {
                    new ChatMessage()
                    {
                        Role = "user",
                        Content = "Write a 100 word long short story in La Fontaine style."
                    }
                },
                Temperature = 0.7f,
            };
    
            openai.CreateChatCompletionAsync(req, 
                (responses) => {
                    var result = string.Join("", responses.Select(response => response.Choices[0].Delta.Content));
                    text.text = result;
                    Debug.Log(result);
                }, 
                () => {
                    Debug.Log("completed");
                }, 
                new CancellationTokenSource()
            );
            /*
            var message = new List<ChatMessage>
            {
                new ChatMessage()
                {
                    Role = "user",
                    Content = "Write a 100 word long short story in La Fontaine style."
                }
            };
            
            openai.CreateChatCompletionAsync(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo",
                Messages = message,
                Stream = true
            }, HandleResponse, null, token);
            */

            button.enabled = true;
        }

        private void HandleResponse(List<CreateChatCompletionResponse> responses)
        {
            text.text = string.Join("", responses.Select(r => r.Choices[0].Delta.Content));
        }

        private void OnDestroy()
        {
            token.Cancel();
        }
    }
}
