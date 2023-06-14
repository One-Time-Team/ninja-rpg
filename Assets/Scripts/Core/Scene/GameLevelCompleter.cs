using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Scene
{
    public class GameLevelCompleter : MonoBehaviour
    {
        private void LoadNextLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            int nextSceneIndex = currentSceneIndex + 1;
            if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
                nextSceneIndex = 0;

            SceneManager.LoadScene(nextSceneIndex); 
        }
    }
}