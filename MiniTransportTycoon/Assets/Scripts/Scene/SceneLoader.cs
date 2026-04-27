using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene
{
    public class SceneLoader
    {
        public static float LoadingProgress;
        public static async Task LoadSceneWithLoadingScreen(string targetScene, string loadingScene)
        {
            SceneManager.LoadScene(loadingScene);
            await Task.Yield();

            AsyncOperation op = SceneManager.LoadSceneAsync(targetScene);
            op.allowSceneActivation = false;

            while (op.progress < 0.9f)
            {
                LoadingProgress = MathF.Round(op.progress * 100f);
                await Task.Yield();
            }
        
            LoadingProgress = 100;

            op.allowSceneActivation = true;
            LoadingProgress = 0;
            await Task.Yield();
        }
    }
}
