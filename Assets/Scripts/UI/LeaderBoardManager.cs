using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Newtonsoft.Json;

public class LeaderBoardManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField _saveRecordInputField;
    [SerializeField] private TMP_Text _warningText;
    [SerializeField] private Button _submitButton;
    [SerializeField] private Button _testButton;
    [SerializeField] private TMP_Text _highScoreText;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private GameManager _gameManager;

    private void Start()
    {
        _testButton.onClick.AddListener(GetLeaderBoard);
        _gameManager.OnGameOver.AddListener(GameOver);
        _submitButton.onClick.AddListener(SubmitScore);
    }
    
    private void SubmitScore()
    {
        if (string.IsNullOrEmpty(_saveRecordInputField.text))
        {
            _warningText.text = "Nickname cannot be empty";
        }
        else
        {
            var leaderboard = PlayerPrefs.GetString("LeaderBoard");
            if (!string.IsNullOrEmpty(leaderboard))
            {
                
                
                
                List<LeaderBoard> leaderBoards = JsonConvert.DeserializeObject<List<LeaderBoard>>(leaderboard);
                leaderBoards.Add(new LeaderBoard(_saveRecordInputField.text, DateTime.ParseExact(_uiManager.TimerText.text, "HH:mm:ss.FFF",
                    CultureInfo.InvariantCulture)));
            }
            else
            {
                
                List<LeaderBoard> leaderBoards = new List<LeaderBoard>()
                {
                    new(_saveRecordInputField.text, DateTime.ParseExact(_uiManager.TimerText.text, "HH:mm:ss.FFF",
                        CultureInfo.InvariantCulture))

                };

                var leaderBoardsString = JsonConvert.SerializeObject(leaderBoards);
                PlayerPrefs.SetString("LeaderBoard", leaderBoardsString);
            }
            PlayerPrefs.Save();
        }
    }

    private void GetLeaderBoard()
    {
        var leaderboard = PlayerPrefs.GetString("LeaderBoard");
        var leadBoard = JsonConvert.DeserializeObject<List<LeaderBoard>>(leaderboard);
        
        leadBoard.Sort((x, y) => DateTime.Compare(x.Time, y.Time));
        
        
        Debug.Log("end");
        
        
    }

    private void GameOver()
    {
        _highScoreText.text = "Score: " + _uiManager.TimerText.text;
    }
}

public class LeaderBoard
{
    public string NickName;
    public DateTime Time;

    public LeaderBoard(string nickName, DateTime time)
    {
        NickName = nickName;
        Time = time;
    }
}
