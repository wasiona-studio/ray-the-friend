using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MyUtilities
{
    public class InitScene : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private SlicedFilledImage image;
        [SerializeField] private string sceneName;
        [SerializeField] private bool hasEnviro,hasMultipleLevels;
        
        private void Awake()
        {
            if (hasMultipleLevels)
            {
                var level = PlayerPrefs.GetInt("LastLevel");
                print(level);
                if (level < 1)
                    Util.SceneUtil.LoadSceneWithProgress(level + 1, panel, image);
                else
                    Util.SceneUtil.LoadSceneWithProgress(level, panel, image);
            }
            else
            {
                Util.SceneUtil.LoadSceneWithProgress(sceneName,panel,image);
            }

            if(hasEnviro)
                Util.SceneUtil.LoadSceneNoProgress("Enviro",LoadSceneMode.Additive);
        }
    }
}