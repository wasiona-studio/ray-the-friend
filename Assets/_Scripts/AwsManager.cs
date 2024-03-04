using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.Runtime;
using MyUtilities;
using UnityEngine;
using UnityEngine.Networking;

public class AwsManager : MonoBehaviour
{
    public static Action<string, AudioSource, Action> OnSpeak;
    public static Action OnSpeakStart, OnSpeakStop;

    private void OnEnable()
    {
        OnSpeak += Speak;
    }

    private void OnDisable()
    {
        OnSpeak -= Speak;
    }

    private async void Speak(string message, AudioSource source, Action onTalk)
    {
        var credentials =
            new BasicAWSCredentials(ChatGptManager.Instance.keys.aws_ak, ChatGptManager.Instance.keys.aws_sk);
        var client = new AmazonPollyClient(credentials, RegionEndpoint.EUCentral1);

        var request = new SynthesizeSpeechRequest()
        {
            Text = message,
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
            source.clip = clip;
            source.Play();
            
            OnSpeakStart?.Invoke();
            StartCoroutine(Util.AudioWait(source, () => OnSpeakStop?.Invoke()));
            
            onTalk();
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
}