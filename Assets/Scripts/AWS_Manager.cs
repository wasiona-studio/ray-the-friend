using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.Runtime;
using UnityEngine;
using UnityEngine.Networking;

public class AWS_Manager : MonoBehaviour
{
    public async void Speak(string message,AudioSource source,Action onTalk)
    {
        var credentials = new BasicAWSCredentials("AKIAQZOKWDM7OA4JBL7H", "yf6rcK8BQH9jkrI8AQo0aivlSGbSoL30lnwNotEt");
        var client = new AmazonPollyClient(credentials, RegionEndpoint.EUCentral1);

        var request = new SynthesizeSpeechRequest()
        {
            Text = message,
            Engine = Engine.Neural,
            VoiceId = VoiceId.Kevin,//Kevin
            OutputFormat = OutputFormat.Mp3
        };

        var response = await client.SynthesizeSpeechAsync(request);
        
        WriteToFile(response.AudioStream);

        using (var www = UnityWebRequestMultimedia.GetAudioClip($"{Application.persistentDataPath}/audio.mp3",AudioType.MPEG))
        {
            var op = www.SendWebRequest();
            while (!op.isDone)
            {
                await Task.Yield();
            }

            var clip = DownloadHandlerAudioClip.GetContent(www);
            source.clip = clip;
            source.Play();
            onTalk?.Invoke();
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