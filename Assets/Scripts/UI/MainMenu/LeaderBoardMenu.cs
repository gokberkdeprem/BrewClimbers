using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LeaderBoardMenu : MonoBehaviour
    {
        [SerializeField] private Button _leaderBoardButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _hiddenCloseButton;

        [SerializeField] private GameObject _leaderboardObject;
        [SerializeField] private GameObject _titleRibbon;
        [SerializeField] private GameObject _titleRibbonContainer;
        
        void Start()
        {
            _leaderBoardButton.onClick.AddListener(ListLeaderBoard);
            _closeButton.onClick.AddListener(ToggleSettings);
            _hiddenCloseButton.onClick.AddListener(ToggleSettings);
        }
        private void ListLeaderBoard()
        {
            var leaderboardString = PlayerPrefs.GetString("LeaderBoard");

            if (string.IsNullOrEmpty(leaderboardString))
            {
                foreach(Transform child in _titleRibbonContainer.transform)
                {
                    Destroy(child.gameObject);
                }
                
                var item = Instantiate(_titleRibbon, _titleRibbonContainer.transform, false);
                item.GetComponentInChildren<TMP_Text>().text = "No Records Yet";
                
            }
            else
            {
                var leaderboard = JsonConvert.DeserializeObject<List<LeaderBoard>>(leaderboardString);

                for (var i = 0; i < leaderboard.Count; i++ )
                {
                    var item = Instantiate(_titleRibbon, _titleRibbonContainer.transform, false);
                    item.GetComponentInChildren<TMP_Text>().text =
                        $"{i + 1}. {leaderboard[i].NickName} {leaderboard[i].Time}";
                } 
            }
        }
        
        private void ToggleSettings()
        {
            _leaderboardObject.SetActive(false);
        }
    }
}
