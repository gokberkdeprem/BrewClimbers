using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class LeaderBoardManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _saveRecordInputField;
        [SerializeField] private TMP_Text _warningText;
        [SerializeField] private Button _submitButton;
        [SerializeField] private Image _submitButtonImage;
        [SerializeField] private TMP_Text _highScoreText;
        [SerializeField] private SinglePlayerUIManager _singlePlayerUIManager;
        [SerializeField] private GameManager _gameManager;
        

        private void Start()
        {
            _gameManager.OnGameOver.AddListener(GameOver);
            _submitButton.onClick.AddListener(SubmitScore);
        }
    
        private void SubmitScore()
        {
            if (string.IsNullOrEmpty(_saveRecordInputField.text))
            {
                _warningText.text = "Nickname cannot be empty";
                StartCoroutine(ChangeSubmitButtonColor());
            }
            else
            {
                var leaderboard = PlayerPrefs.GetString("LeaderBoard");
                if (string.IsNullOrEmpty(leaderboard))
                {
                    List<LeaderBoard> leaderBoards = new List<LeaderBoard>()
                    {
                        new(_saveRecordInputField.text, TimeSpan.FromSeconds(_singlePlayerUIManager.Timer))
                    };

                    var leaderBoardsString = JsonConvert.SerializeObject(leaderBoards);
                    PlayerPrefs.SetString("LeaderBoard", leaderBoardsString);
                }
                else
                {
                    List<LeaderBoard> leaderBoards = JsonConvert.DeserializeObject<List<LeaderBoard>>(leaderboard);
                    leaderBoards.Add(new LeaderBoard(_saveRecordInputField.text, TimeSpan.FromSeconds(_singlePlayerUIManager.Timer)));
                    leaderBoards.Sort((x, y) => TimeSpan.Compare(x.Time, y.Time));
                
                    var leaderBoardsString = JsonConvert.SerializeObject(leaderBoards);
                    PlayerPrefs.SetString("LeaderBoard", leaderBoardsString);
                }
                PlayerPrefs.Save();

                _submitButtonImage.color = Color.green;
                _submitButton.enabled = false;
                _warningText.text = "Record is successfully saved.";
                
            }
        }

        private void GameOver()
        {
            _highScoreText.text = "Score: " + _singlePlayerUIManager.TimerText.text;
        }
        
        
        private IEnumerator ChangeSubmitButtonColor()
        {
            var temp = _submitButton.image.color;
            _submitButtonImage.color = Color.red;
            yield return new WaitForSeconds(1);
            _submitButtonImage.color = temp;
            _singlePlayerUIManager.ToggleSaveScorePanel();
        }
    }

    public class LeaderBoard
    {
        public string NickName;
        public TimeSpan Time;

        public LeaderBoard(string nickName, TimeSpan time)
        {
            NickName = nickName;
            Time = time;
        }
    }
}