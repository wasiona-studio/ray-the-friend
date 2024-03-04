using System;
using System.Collections;
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

        public static Action OnRecordingStart, OnRecordingEnd;
        
        private void Start()
        {
            recordButton.onClick.AddListener(OnRecordingStart.Invoke);
        }

        private void OnEnable()
        {
            OnRecordingStart += StartRecording;
            OnRecordingEnd += EndRecording;
        }

        private void OnDisable()
        {
            OnRecordingStart -= StartRecording;
            OnRecordingEnd -= EndRecording;
        }

        private void StartRecording()
        {
            
            StartCoroutine(Recording());
            recordButton.enabled = false;

#if !UNITY_WEBGL
            _clip = Microphone.Start(Microphone.devices[0], false, _duration, 44100);
#endif
        }

        private async void EndRecording()
        {
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
            var res = await ChatGptManager.Instance.OpenAi.CreateAudioTranscription(req);
            
            ChatGptManager.OnAskGpt?.Invoke(res.Text,true);
            
            message.text = res.Text;
            progressBar.fillAmount = 0;
            recordButton.enabled = true;
        }

        private IEnumerator Recording()
        {
            while (_time < _duration)
            {
                _time += Time.deltaTime;
                progressBar.fillAmount = _time / _duration;
                
                yield return null;
            }

            _time = 0;
            OnRecordingEnd?.Invoke();
        }
        
        /*
        private void Update()
        {
            if (!_isRecording) return;

            _time += Time.deltaTime;
            progressBar.fillAmount = _time / _duration;

            if (_time < _duration) return;

            _time = 0;
            _isRecording = false;
            onRecordingEnd?.Invoke();
        }*/
    }
}