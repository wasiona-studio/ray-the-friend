using System;
using System.Collections.Generic;
using System.Linq;
using MyUtilities;
using RayTheFriend.AWS;
using RayTheFriend.Extras;
using RayTheFriend.GPT;
using RayTheFriend.SO;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

namespace RayTheFriend.Managers
{
    public class AnimationManager : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Animations animations;
        [SerializeField] public List<EmotionsSO> emotions = new();
        private bool emotionAvailable;

        private void OnEnable()
        {
            Whisper.OnRecordingStart += () => { PlayAnimation(animations.listening); };
            Whisper.OnRecordingEnd += () => { PlayAnimation(animations.thinking); };
            AwsManager.OnSpeakStart += () =>
            {
                if (!emotionAvailable)
                    PlayAnimation(animations.explaining);
            };
            AwsManager.OnSpeakStop += () => { PlayAnimation(animations.idle); };
        }

        private void Awake()
        {
            AddEmotionsToStartPrompt();
        }

        public void TriggerEmotion(string key)
        {
            var emotion = emotions.FirstOrDefault(a => a.key == key);

            if (emotion != null)
            {
                emotionAvailable = true;
                PlayAnimation(emotion.animationData, () => { PlayAnimation(animations.idle); });
            }
            else
            {
                emotionAvailable = false;
                Util.Log($"Key: {key} does not exist", Color.red, 12);
            }
        }

        private void AddEmotionsToStartPrompt()
        {
            ChatGptManager.Instance.startingPrompt += "Use these tags as animation ques: ";
            foreach (var emotion in emotions)
            {
                ChatGptManager.Instance.startingPrompt += emotion.key + ",";
            }

            print(ChatGptManager.Instance.startingPrompt);
        }

        private void PlayAnimation(AnimationData animationDataToPlay)
        {
            animator.CrossFade(animationDataToPlay.clip.name, animationDataToPlay.clipCrossFadeDuration);
        }

        private async void PlayAnimation(AnimationData animationDataToPlay, Action onClipFinish)
        {
            animator.CrossFade(animationDataToPlay.clip.name, animationDataToPlay.clipCrossFadeDuration);
            var time = animationDataToPlay.clip.length;
            await Task.Delay((int)(time * 1000));
            onClipFinish();
        }
    }
}