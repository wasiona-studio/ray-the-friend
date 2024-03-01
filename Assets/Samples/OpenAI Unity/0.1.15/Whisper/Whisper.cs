using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OpenAI
{
    public class Whisper : MonoBehaviour
    {
        [SerializeField] private Button recordButton;
        [SerializeField] private Image progressBar;
        [SerializeField] private TextMeshProUGUI message;

        private readonly string _fileName = "output.wav";
        private readonly int _duration = 5;

        private AudioClip _clip;
        private bool _isRecording;
        private float _time;

        private void Start()
        {
            recordButton.onClick.AddListener(StartRecording);
        }

        private void StartRecording()
        {
            _isRecording = true;
            recordButton.enabled = false;

#if !UNITY_WEBGL
            _clip = Microphone.Start(Microphone.devices[0], false, _duration, 44100);
#endif
        }

        private async void EndRecording()
        {
            print("waiting whisper response");
            message.text = "...";

#if !UNITY_WEBGL
            Microphone.End(null);
#endif

            byte[] data = SaveWav.Save(_fileName, _clip);

            var req = new CreateAudioTranscriptionsRequest
            {
                FileData = new FileData() { Data = data, Name = "audio.wav" },
                Model = "whisper-1",
                Language = "en"
            };
            var res = await ChatGptManager.instance.OpenAi.CreateAudioTranscription(req);
            GetComponent<ChatGptManager>().AskChatGPT(res.Text);
            progressBar.fillAmount = 0;
            message.text = res.Text;
            print($"USER: {message.text}");
            recordButton.enabled = true;
        }

        private void Update()
        {
            if (!_isRecording) return;

            _time += Time.deltaTime;
            progressBar.fillAmount = _time / _duration;

            if (_time < _duration) return;

            _time = 0;
            _isRecording = false;
            EndRecording();
        }
    }
}