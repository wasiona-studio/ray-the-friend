using RayTheFriend.GPT;
using UnityEngine;

namespace RayTheFriend
{
    public class LsTesting : MonoBehaviour
    {
        [TextArea] [Tooltip("In play mode press 'S' to test the speech and lipsyncing")] [SerializeField]
        private string testSpeechText;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                ChatGptManager.Instance.PlaySpeech(testSpeechText);
            }
        }
    }
}