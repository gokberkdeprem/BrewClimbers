using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class SinglePlayerUIManager : MonoBehaviour
    {
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private GameObject _leaderboardUserInputPanel;
    
        // UITimer Canvas
        public float Timer;
        public TMP_Text TimerText; 
    
        // Gameplay Canvas
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _respawnButton;
    
        // Pause Menu Canvas
        [SerializeField] private GameObject _pauseMenuCanvas;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _pauseMenuRestartButton;
        [SerializeField] private Button _pauseMainMenuButton;
        [SerializeField] private Button _hiddenCloseButton;
    
        // End Game Menu Canvas
        [SerializeField] private GameObject _endMenuCanvas;
        [SerializeField] private Button _saveScoreButton;
        [SerializeField] private Button _closeSaveScorePanel;
        [SerializeField] private Button _hiddenCloseSaveScorePanel;
        [SerializeField] private TMP_Text _endGameScoreText;
        [SerializeField] private Button _endMenuRestartButton;
        [SerializeField] private Button _endMenuMainMenuButton;
        
        // TabCountCanvas
        [SerializeField] private TMP_Text _tabCountText;
    
        private void Start()
        {
            _gameManager.OnGameOver.AddListener(GameOver);
        
            // UITimer Canvas
            Timer = 0;
        
            // Gameplay Canvas
            _pauseButton.onClick.AddListener(TogglePauseMenu);
        
            // Pause Menu Canvas
            _continueButton.onClick.AddListener(TogglePauseMenu);
            _pauseMenuRestartButton.onClick.AddListener(ReloadScene);
            _pauseMainMenuButton.onClick.AddListener(LoadMainMenu);
            _hiddenCloseButton.onClick.AddListener(TogglePauseMenu);
        
            // End Game Menu Canvas
            _saveScoreButton.onClick.AddListener(ToggleSaveScorePanel);
            _closeSaveScorePanel.onClick.AddListener(ToggleSaveScorePanel);
            // _hiddenCloseSaveScorePanel.onClick.AddListener(ToggleSaveScorePanel);
            _endMenuRestartButton.onClick.AddListener(ReloadScene);
            _endMenuMainMenuButton.onClick.AddListener(LoadMainMenu);
        
            // Set canvas disabled on the start
            _pauseMenuCanvas.SetActive(false);
            _endMenuCanvas.SetActive(false);
        }
        void Update () {

            if(!_gameManager.IsGameOver){
                Timer += Time.deltaTime;
                int minutes = Mathf.FloorToInt(Timer / 60F);
                int seconds = Mathf.FloorToInt(Timer % 60F);
                int milliseconds = Mathf.FloorToInt((Timer * 100F) % 100F);
                TimerText.text = minutes.ToString ("00") + ":" + seconds.ToString ("00") + ":" + milliseconds.ToString("00");
            }
        }
    
        void TogglePauseMenu()
        {
            if (_pauseMenuCanvas.activeInHierarchy)
                _pauseMenuCanvas.SetActive(false);
            else
                _pauseMenuCanvas.SetActive(true);
        }

        public void ToggleSaveScorePanel()
        {
            if (_leaderboardUserInputPanel.activeInHierarchy)
                _leaderboardUserInputPanel.SetActive(false);
            else
                _leaderboardUserInputPanel.SetActive(true);
        }

        void GameOver()
        {
            _endMenuCanvas.SetActive(true);
            _endGameScoreText.text = TimerText.text;
        }
        
        private void LoadMainMenu()
        {
            SceneManager.LoadScene(0);
        }
        
        private void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
