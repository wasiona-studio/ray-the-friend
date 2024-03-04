using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

//by Dimitrije Denic
//Requires DOTween

namespace MyUtilities
{
    public static class Util
    {
        public static readonly RayCastUtil RayUtils = new RayCastUtil();

        public static readonly SceneUtil SceneUtil = new SceneUtil();

        public static readonly MoveUtil MoveUtils = new MoveUtil();

        public static readonly UIUtil UiUtil = new UIUtil();

        public static readonly ParentChildUtil ParentChildUtil = new ParentChildUtil();


        /// <summary>
        /// Starts a given method after set delay. If method doesn't have parameters, pass just the method name, if the method has parameters, use Lambda expression for calling the method.
        /// </summary>
        /// <param name="delay">Delay of the task in seconds</param>
        /// <param name="methodToRun">Method with or without parameters that is run after delay</param>
        public static async void DoTaskDelayed(float delay, Action methodToRun)
        {
            await System.Threading.Tasks.Task.Delay(Mathf.RoundToInt(delay * 1000));
            methodToRun();
        }

        /// <summary>
        /// Checks if a GameObject has been destroyed.
        /// </summary>
        /// <param name="gameObject">GameObject reference to check</param>
        /// <returns>If the game object has been marked as destroyed by UnityEngine</returns>
        public static bool IsDestroyed(this GameObject gameObject)
        {
            // UnityEngine overloads the == operator for the GameObject type
            // and returns null when the object has been destroyed, but 
            // actually the object is still there but has not been cleaned up yet
            // if we test both we can determine if the object has been destroyed.
            return gameObject == null && !ReferenceEquals(gameObject, null);
        }

        /// <summary>
        /// Calculates a visual center point of an object with all of its children
        /// </summary>
        /// <param name="obj">Parent object</param>
        /// <returns>Vector3 position of the center</returns>
        public static Vector3 CalculateCentroid(GameObject obj)
        {
            var meshFilters = obj.GetComponentsInChildren<MeshFilter>();
            var numMeshFilters = meshFilters.Length;

            var sum = Vector3.zero;
            foreach (var mf in meshFilters)
            {
                sum += mf.transform.position;
            }

            var centroid = sum / numMeshFilters;

            return centroid;
        }

        /// <summary>
        /// Adds suffix to numbers above 1000
        /// </summary>
        /// <param name="val">value to add suffix to</param>
        /// <param name="format">string format</param>
        /// <param name="decimals">Number of numbers after the decimal point</param>
        /// <returns>new string formatted for large numbers</returns>
        public static string WithSuffix(this float val, string format, int decimals = 2)
        {
            if (val >= 1000)
            {
                if (val >= 1000000)
                {
                    if (val >= 1000000000)
                    {
                        if (val >= 1000000000000)
                        {
                            return $"{Math.Round(val / 1000000000000f, decimals)}T";
                        }

                        return $"{Math.Round(val / 1000000000f, decimals)}B";
                    }

                    return $"{Math.Round(val / 1000000f, decimals)}M";
                }

                return $"{Math.Round(val / 1000f, decimals)}K";
            }

            return val.ToString(format);
        }

        /// <summary>
        /// Sets Vector to a random Vector3
        /// </summary>
        /// <param name="v">Vector to set</param>
        /// <param name="min">Minimum vector value</param>
        /// <param name="max">Maximum vector value</param>
        public static void RandomVector(this ref Vector3 v, Vector3 min, Vector3 max)
        {
            v = new Vector3(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y),
                UnityEngine.Random.Range(min.z, max.z));
        }


        /// <summary>
        /// Formats given seconds float to time HH:MM:SS 
        /// </summary>
        /// <param name="seconds">seconds to format</param>
        /// <returns></returns>
        public static string FormatTime(this float seconds)
        {
            var timeSpan = TimeSpan.FromSeconds(seconds);

            var formattedTime = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";

            return formattedTime;
        }

        /// <summary>
        /// Logs with custom color, font size, bold or italic
        /// </summary>
        /// <param name="text">Text to log</param>
        /// <param name="fontSize">Font size of the text to log</param>
        /// <param name="bold">Is the text bold</param>
        /// <param name="italic">Is the text italic</param>
        /// <param name="color">Color of the text to log</param>
        public static void Log(string text, Color color, float fontSize = 10, bool bold = false, bool italic = false)
        {
            if (bold)
                text = $"<b>{text}</b>";

            if (italic)
                text = $"<i>{text}</i>";

            text = $"<size={fontSize}><color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color></size>";

            Debug.Log(text);
        }

        /// <summary>
        /// Calls a method when the audio finishes playing
        /// </summary>
        /// <param name="source">Audio source we are listening to</param>
        /// <param name="onAudioFinish">method to play when the audio finishes</param>
        /// <returns></returns>
        public static IEnumerator AudioWait(AudioSource source,Action onAudioFinish)
        {
            yield return new WaitForSeconds(source.clip.length);
            onAudioFinish();
        }
    }
}