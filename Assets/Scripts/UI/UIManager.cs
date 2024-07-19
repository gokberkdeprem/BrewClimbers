using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private GameObject _leaderboardUserInputPanel;
    
        // UITimer Canvas
        public float Timer;
        public TMP_Text TimerText; 
    
        // Gameplay Canvas
        [SerializeField] private Button _pauseButton;
    
        // Pause Menu Canvas
        [SerializeField] private GameObject _pauseMenuCanvas;
        [SerializeField] private Button _continueButton;
    
        // End Game Menu Canvas
        [SerializeField] private GameObject _endMenuCanvas;
        [SerializeField] private Button _saveScoreButton;
        [SerializeField] private Button _closeSaveScoreInputPanel;
    
        private void Start()
        {
            _gameManager.OnGameOver.AddListener(GameOver);
        
            // UITimer Canvas
            Timer = 0;
        
            // Gameplay Canvas
            _pauseButton.onClick.AddListener(TogglePauseMenu);
        
            // Pause Menu Canvas
            _continueButton.onClick.AddListener(TogglePauseMenu);
        
            // End Game Menu Canvas
            _saveScoreButton.onClick.AddListener(ToggleSaveLeaderBoardPanel);
            _closeSaveScoreInputPanel.onClick.AddListener(ToggleSaveLeaderBoardPanel);
        
            // Set unused canvas disabled
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

        void ToggleSaveLeaderBoardPanel()
        {
            if (_leaderboardUserInputPanel.activeInHierarchy)
                _leaderboardUserInputPanel.SetActive(false);
            else
                _leaderboardUserInputPanel.SetActive(true);
        }

        void GameOver()
        {
            _endMenuCanvas.SetActive(true);
        }
    
    }
}
