using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyUtilities
{
    public class SceneUtil
    {
        /// <summary>
        /// Loads given scene with loading bar
        /// </summary>
        /// <param name="sceneName">String name of the scene to load</param>
        /// <param name="loaderCanvas">Canvas that contains the progress bar and other UI elements</param>
        /// <param name="progressBar">Progress bar that gets filled</param>
        public void LoadSceneWithProgress(string sceneName, GameObject loaderCanvas, SlicedFilledImage progressBar)
        {
            var scene = SceneManager.LoadSceneAsync(sceneName);
            
            scene.allowSceneActivation = false;

            loaderCanvas.SetActive(true);
            do
            {
                progressBar.fillAmount = scene.progress;
            } while (scene.progress < 0.9f);

            scene.allowSceneActivation = true;
            loaderCanvas.SetActive(false);
        }
        
        /// <summary>
        /// Loads given scene with loading bar
        /// </summary>
        /// <param name="sceneIndex">Index of the scene to load</param>
        /// <param name="loaderCanvas">Canvas that contains the progress bar and other UI elements</param>
        /// <param name="progressBar">Progress bar that gets filled</param>
        public void LoadSceneWithProgress(int sceneIndex, GameObject loaderCanvas, SlicedFilledImage progressBar)
        {
            var scene = SceneManager.LoadSceneAsync(sceneIndex);
            scene.allowSceneActivation = false;

            loaderCanvas.SetActive(true);
            do
            {
                progressBar.fillAmount = scene.progress;
            } while (scene.progress < 0.9f);

            scene.allowSceneActivation = true;
            loaderCanvas.SetActive(false);
        }

        /// <summary>
        /// Loads given scene
        /// </summary>
        /// <param name="sceneName">String name of the scene to load</param>
        /// <param name="sceneMode">Mode to load the scene in</param>
        public void LoadSceneNoProgress(string sceneName,LoadSceneMode sceneMode = LoadSceneMode.Single)
        {
            SceneManager.LoadSceneAsync(sceneName,sceneMode);
        }
        
        /// <summary>
        /// Loads given scene
        /// </summary>
        /// <param name="sceneIndex">Index of the scene to load</param>
        /// <param name="sceneMode">Mode to load the scene in</param>

        public void LoadSceneNoProgress(int sceneIndex,LoadSceneMode sceneMode = LoadSceneMode.Single)
        {
            SceneManager.LoadSceneAsync(sceneIndex,sceneMode);
        }
        
        /// <summary>
        /// Get the name of a scene by its build index
        /// </summary>
        /// <param name="buildIndex">index from the build settings of the scene to get</param>
        /// <returns>Name of the scene you are searching for</returns>
        public static string NameOfSceneByBuildIndex(int buildIndex)
        {
            var path = SceneUtility.GetScenePathByBuildIndex(buildIndex);
            var slash = path.LastIndexOf('/');
            var name = path.Substring(slash + 1);
            var dot = name.LastIndexOf('.');
            return name.Substring(0, dot);
        }
    }
}