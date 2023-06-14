using Core.Services.Updater;
using UnityEngine;

namespace Core.Scene
{
    public class GameLevelStarter : MonoBehaviour
    {
        [SerializeField] private GameObject _uIInputView;
        [SerializeField] private GameObject _startingCamera;

        private ProjectUpdater _projectUpdater;

        private void Awake()
        {
            _uIInputView.SetActive(false);
            _startingCamera.SetActive(false);
        }
        
        private void Start()
        {
            _projectUpdater = FindObjectOfType<ProjectUpdater>();
            _projectUpdater.enabled = false;
        }

        private void StartLevel()
        {
            _startingCamera.SetActive(true);
            _uIInputView.SetActive(true);
            _projectUpdater.enabled = true;

            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                StartLevel();
        }
    }
}