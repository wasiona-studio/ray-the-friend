using UnityEngine;

public class LSTesting : MonoBehaviour
{
    [TextArea] [Tooltip("In play mode press 'S' to test the speech and lipsyncing")]
    [SerializeField] private string testSpeechText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            ChatGptManager.instance.TestSpeech(testSpeechText);
        }
    }
}