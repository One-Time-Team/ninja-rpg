using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Scene
{
    public class GameLevelRestarter : MonoBehaviour
    {
        private void RestartLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex); 
        }
    }
}