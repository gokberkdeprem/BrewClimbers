using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
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
        [SerializeField] private Button _ropeSwingButton;
        [SerializeField] private TMP_Text _ropeSwingText;
        [SerializeField] private Button _tapJumpTabButton;
        [SerializeField] private TMP_Text _tapJumpText;
        [SerializeField] private LeaderboardTab _currentTab;
        [SerializeField] private  LeaderboardTab _defaultTab = LeaderboardTab.RopeSwing;
        [SerializeField] private LB _lb;
        private bool isPopulated;
        
        void Start()
        {
            ListLeaderBoard(_defaultTab);
            _lb.OnEnabled.AddListener(delegate { ListLeaderBoard(_defaultTab); });
            _leaderBoardButton.onClick.AddListener(delegate { ListLeaderBoard(_defaultTab); });
            _ropeSwingButton.onClick.AddListener(delegate { ListLeaderBoard(LeaderboardTab.RopeSwing); });
            _tapJumpTabButton.onClick.AddListener(delegate { ListLeaderBoard(LeaderboardTab.TapJump); });
            _closeButton.onClick.AddListener(ToggleSettings);
            _hiddenCloseButton.onClick.AddListener(ToggleSettings);
        }

        private void ListLeaderBoard(LeaderboardTab? currentTab = default)
        {
            var leaderboardString = String.Empty;
            
            switch (currentTab)
            {
                case LeaderboardTab.RopeSwing:
                    leaderboardString = PlayerPrefs.GetString("RopeSwingLeaderBoard");
                    _ropeSwingText.color = Color.yellow;
                    _tapJumpText.color = Color.white;
                    break;
                case LeaderboardTab.TapJump:
                    leaderboardString = PlayerPrefs.GetString("TapJumpLeaderBoard");
                    _ropeSwingText.color = Color.white;
                    _tapJumpText.color = Color.yellow;
                    break;
            }
            
            foreach(Transform child in _titleRibbonContainer.transform)
            {
                Destroy(child.gameObject);
            }
            
            if (string.IsNullOrEmpty(leaderboardString))
            {
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
                isPopulated = true;
            }
        }
        
        private void ToggleSettings()
        {
            _leaderboardObject.SetActive(false);
        }
    }
}
