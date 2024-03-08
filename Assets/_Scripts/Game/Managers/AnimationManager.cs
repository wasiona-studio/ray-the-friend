using System.Collections.Generic;
using System.Linq;
using MyUtilities;
using RayTheFriend.AWS;
using RayTheFriend.Extras;
using RayTheFriend.GPT;
using RayTheFriend.SO;
using UnityEngine;

namespace RayTheFriend.Managers
{
    public class AnimationManager : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Animations animations;
        [SerializeField] public List<EmotionsSO> emotions = new();

        private void OnEnable()
        {
            Whisper.OnRecordingStart += () => { PlayAnimation(animations.listening); };
            Whisper.OnRecordingEnd += () => { PlayAnimation(animations.thinking); };
            AwsManager.OnSpeakStart += () => { PlayAnimation(animations.explaining); };
            AwsManager.OnSpeakStop += () => { PlayAnimation(animations.idle); };
        }

        private void Awake()
        {
            AddEmotionsToStartPrompt();
        }
        public void TriggerEmotion(string key)
        {
            print(key);
            var emotion = emotions.FirstOrDefault(a => a.key == key);

            if (emotion != null)
                PlayAnimation(emotion.animationData);
            else
                Util.Log($"Key: {key} does not exist", Color.red, 12);
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
    }
}