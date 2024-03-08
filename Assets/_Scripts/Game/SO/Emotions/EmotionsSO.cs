using UnityEngine;
using RayTheFriend.Extras;

namespace RayTheFriend.SO
{
    [CreateAssetMenu(fileName = "Emotion", menuName = "EmotionAnimations")]
    public class EmotionsSO : ScriptableObject
    {
        [Tooltip("Write key in a '#key#' pattern")]
        public string key;

        public AnimationData animationInfo;
    }
}