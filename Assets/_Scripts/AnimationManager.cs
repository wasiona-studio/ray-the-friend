using System;
using OpenAI;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float clipCrossFadeDuration = 0.1f;
    [SerializeField] private Animations animations;
    
    private void OnEnable()
    {
        Whisper.OnRecordingStart += () =>
        {
            PlayAnimation(animations.listening);
        };
        Whisper.OnRecordingEnd += () =>
        {
            PlayAnimation(animations.thinking);
        };
        AwsManager.OnSpeakStart += () =>
        {
            PlayAnimation(animations.explaining);
        };
        AwsManager.OnSpeakStop += () =>
        {
            PlayAnimation(animations.idle);
        };
    }

    private void RecordAction()
    {
        
    }
    private void PlayAnimation(AnimationClip animationToPlay)
    {
        if(animationToPlay == null) return;
        
        animator.CrossFade(animationToPlay.name,clipCrossFadeDuration);
    }
}
[Serializable]
public class Animations
{
    public AnimationClip idle, listening, thinking, explaining;
}