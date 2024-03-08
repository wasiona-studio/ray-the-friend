using System;
using UnityEngine;

namespace RayTheFriend.Extras
{
    [Serializable]
    internal class Animations
    {
        public AnimationData idle, listening, thinking, explaining;
    }

    [Serializable]
    public struct AnimationData
    {
        public AnimationClip clip;
        public float clipCrossFadeDuration;
    }
}