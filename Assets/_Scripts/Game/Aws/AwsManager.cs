using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.Runtime;
using DG.Tweening;
using MyUtilities;
using RayTheFriend.GPT;
using UnityEngine;
using UnityEngine.Networking;

namespace RayTheFriend.AWS
{
    public class AwsManager : MonoBehaviour
    {
        public static Action<string, AudioSource, Action> OnQueueSpeech;
        public static Action OnSpeakStart, OnSpeakStop;

        [SerializeField] private List<Speech> speechQueue = new();
        private readonly List<string> lastMessages = new();
        private bool queuePlaying;

        private void OnEnable()
        {
            OnQueueSpeech += QueueSpeech;
        }

        private void OnDisable()
        {
            OnQueueSpeech -= QueueSpeech;
        }

        #region Speech Queue

        private void QueueSpeech(string message, AudioSource source, Action onTalk)
        {
            var m = message.Trim();
            if (!lastMessages.Contains(m))
            {
                var s = new Speech(m, source, onTalk);
                speechQueue.Add(s);
                lastMessages.Add(m);
            }

            if (queuePlaying) return;

            queuePlaying = true;
            PlayFroQueue();
        }

        private void PlayFroQueue()
        {
            if (speechQueue.Count <= 0)
            {
                queuePlaying = false;
                lastMessages.Clear();

                DOVirtual.DelayedCall(3, () => { ChatGptManager.Instance.ConversationDone(); });
                return;
            }

            var s = speechQueue[0];

            async void SpeechStarted()
            {
                await Task.Delay((int)(s.Source.clip.length * 1000));
                PlayFroQueue();
            }

            Speak(s, SpeechStarted);
            speechQueue.Remove(s);
        }

        #endregion

        private async void Speak(Speech speech, Action speechStarted)
        {
            var credentials =
                new BasicAWSCredentials(ChatGptManager.Instance.keys.aws_ak, ChatGptManager.Instance.keys.aws_sk);
            var client = new AmazonPollyClient(credentials, RegionEndpoint.EUCentral1);

            var request = new SynthesizeSpeechRequest()
            {
                Text = speech.Message,
                Engine = Engine.Neural,
                VoiceId = VoiceId.Kevin, //Kevin
                OutputFormat = OutputFormat.Mp3
            };

            var response = await client.SynthesizeSpeechAsync(request);

            WriteToFile(response.AudioStream);

            using (var www = UnityWebRequestMultimedia.GetAudioClip($"{Application.persistentDataPath}/audio.mp3",
                       AudioType.MPEG))
            {
                var op = www.SendWebRequest();
                while (!op.isDone)
                {
                    await Task.Yield();
                }

                var clip = DownloadHandlerAudioClip.GetContent(www);
                speech.Source.clip = clip;
                speech.Source.Play();
                speechStarted();
                OnSpeakStart?.Invoke();
                StartCoroutine(Util.AudioWait(speech.Source, () => OnSpeakStop?.Invoke()));

                speech.CompleteAction();
            }
        }

        private void WriteToFile(Stream stream)
        {
            using (var fileStream = new FileStream($"{Application.persistentDataPath}/audio.mp3", FileMode.Create))
            {
                byte[] buffer = new byte[8 * 1024];
                int bytesRead;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fileStream.Write(buffer, 0, bytesRead);
                }
            }
        }

        [Serializable]
        public class Speech
        {
            public string Message;
            public AudioSource Source;
            public Action CompleteAction;

            public Speech(string message, AudioSource source, Action completeAction)
            {
                Message = message;
                Source = source;
                CompleteAction = completeAction;
            }
        }
    }
}