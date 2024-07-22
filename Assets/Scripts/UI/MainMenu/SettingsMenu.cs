using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Button _resetLeaderBoardButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _hiddenCloseButton;
    [SerializeField] private GameObject _settingsObject;
     
    void Start()
    {
        _resetLeaderBoardButton.onClick.AddListener(ResetLeaderBoard);   
        _closeButton.onClick.AddListener(CloseSettings);
        _hiddenCloseButton.onClick.AddListener(CloseSettings);
    }

    private void ResetLeaderBoard()
    {
        if (PlayerPrefs.HasKey("LeaderBoard"))
            PlayerPrefs.DeleteKey("LeaderBoard");
    }

    private void CloseSettings()
    {
        _settingsObject.SetActive(false);
    }
}
