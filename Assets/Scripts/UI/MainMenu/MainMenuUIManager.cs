using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuUIManager : MonoBehaviour
    {
        [SerializeField] private Button _singlePlayer;
        [SerializeField] private Button _multiPlayer;
        [SerializeField] private Button _leaderBoard;
        [SerializeField] private Button _settings;
        [SerializeField] private Button _exit;


        [SerializeField] private GameObject _leaderBoardObject;
        [SerializeField] private GameObject _settingsObject;
        
        // Start is called before the first frame update
        void Start()
        {
            _singlePlayer.onClick.AddListener(delegate { LoadGameScenes(1); });
            // _multiPlayer.onClick.AddListener(delegate { LoadGameScenes(0); });
            _leaderBoard.onClick.AddListener(ToggleLeaderBoard);
            _settings.onClick.AddListener(ToggleSettings);
            _exit.onClick.AddListener(QuitApplication);
        }

        private void QuitApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void ToggleLeaderBoard()
        {
            if(_leaderBoardObject.activeInHierarchy)
                _leaderBoardObject.SetActive(false);
            else
                _leaderBoardObject.SetActive(true);
        }

        private void ToggleSettings()
        {
            if(_settingsObject.activeInHierarchy)
                _settingsObject.SetActive(false);
            else
                _settingsObject.SetActive(true);
        }
        
        private void LoadGameScenes(int sceneNo)
        {
            SceneManager.LoadScene(sceneNo);
        }
    }
}
