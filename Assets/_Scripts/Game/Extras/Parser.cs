using System;
using System.Collections.Generic;
using System.Linq;
using MyUtilities;
using OpenAI;
using RayTheFriend.SO;
using UnityEngine;

namespace RayTheFriend.Extras
{
    public class Parser
    {
        public string inputForParsing;

        public string[] ParseSentence(List<CreateChatCompletionResponse> responses)
        {
            inputForParsing = string.Join("", responses.Select(r => r.Choices[0].Delta.Content));

            if (!inputForParsing.EndsWith(".") && !inputForParsing.EndsWith("!") && !inputForParsing.EndsWith("?"))
                return null;

            return Util.SplitIntoSentences(inputForParsing, @"(?<=[.!?])\s+");
        }

        public string ParseEmotion(List<EmotionsSO> emotions, string sentence, Action<string> onEmotionFound)
        {
            foreach (var e in emotions)
            {
                if (!sentence.Contains(e.key)) continue;
                
                onEmotionFound(e.key);
                return sentence.Replace(e.key, "");
            }

            return sentence;
        }

        public void ClearParseInput()
        {
            inputForParsing = string.Empty;
        }
    }
}