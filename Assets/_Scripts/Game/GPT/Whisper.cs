using System;
using System.Collections;
using OpenAI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RayTheFriend.GPT
{
    public class Whisper : MonoBehaviour
    {
        [SerializeField] private SpeakingUi speakingUi;

        private readonly string _fileName = "output.wav";
        private readonly int _duration = 5;

        private AudioClip _clip;
        private bool _isRecording;
        private float _time;

        public static Action OnRecordingStart, OnRecordingEnd;

        private void Start()
        {
            speakingUi.recordButton.onClick.AddListener(OnRecordingStart.Invoke);
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
            speakingUi.recordButton.image.sprite = speakingUi.sprites.recording;
            speakingUi.recordButton.interactable = false;

#if !UNITY_WEBGL
            _clip = Microphone.Start(Microphone.devices[0], false, _duration, 44100);
#endif
        }

        private async void EndRecording()
        {
            speakingUi.message.text = "...";
            speakingUi.recordButton.image.sprite = speakingUi.sprites.thinking;
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

            ChatGptManager.OnAskGpt?.Invoke(res.Text);

            speakingUi.message.text = res.Text;
            speakingUi.progressBar.fillAmount = 0;
            speakingUi.recordButton.image.sprite = speakingUi.sprites.canRecord;
            speakingUi.recordButton.interactable = true;
        }

        private IEnumerator Recording()
        {
            while (_time < _duration)
            {
                _time += Time.deltaTime;
                speakingUi.progressBar.fillAmount = _time / _duration;

                yield return null;
            }

            _time = 0;
            OnRecordingEnd?.Invoke();
        }
    }

    [Serializable]
    public class SpeakingUi
    {
        public Button recordButton;
        public Image progressBar;
        public TextMeshProUGUI message;
        public Sprites sprites;

        [Serializable]
        public struct Sprites
        {
            public Sprite canRecord, recording, thinking;
        }
    }
}