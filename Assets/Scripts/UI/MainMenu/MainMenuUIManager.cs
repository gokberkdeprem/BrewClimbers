using Enums;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
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


        [SerializeField] private GameObject _leaderBoardPanel;
        [SerializeField] private GameObject _settingsPanel;
        [SerializeField] private GameObject _gameModPanel;
        
        // Start is called before the first frame update
        void Start()
        {
            _singlePlayer.onClick.AddListener(ToggleSelectGameMod);
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
            if(_leaderBoardPanel.activeInHierarchy)
                _leaderBoardPanel.SetActive(false);
            else
                _leaderBoardPanel.SetActive(true);
        }

        private void ToggleSettings()
        {
            if(_settingsPanel.activeInHierarchy)
                _settingsPanel.SetActive(false);
            else
                _settingsPanel.SetActive(true);
        }

        private void ToggleSelectGameMod()
        {
            if(_gameModPanel.activeInHierarchy)
                _settingsPanel.SetActive(false);
            else
                _gameModPanel.SetActive(true);
        }
        
        private void LoadGameScenes(int sceneNo)
        {
            SceneManager.LoadScene(sceneNo);
        }
    }
}
