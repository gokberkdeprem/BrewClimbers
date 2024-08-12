using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Button _resetLeaderBoardButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _hiddenCloseButton;
    [SerializeField] private GameObject _settingsObject;
    [SerializeField] private TMP_Text _applicationVersion;
     
    void Start()
    {
        _resetLeaderBoardButton.onClick.AddListener(ResetLeaderBoard);   
        _closeButton.onClick.AddListener(CloseSettings);
        _hiddenCloseButton.onClick.AddListener(CloseSettings);
        _applicationVersion.text = $"Application Version: {Application.version}";
    }

    private void ResetLeaderBoard()
    {
        if (PlayerPrefs.HasKey("RopeSwingLeaderBoard"))
            PlayerPrefs.DeleteKey("RopeSwingLeaderBoard");
        
        // if (PlayerPrefs.HasKey("TapJumpLeaderBoard"))
        //     PlayerPrefs.DeleteKey("TapJumpLeaderBoard");
    }

    private void CloseSettings()
    {
        _settingsObject.SetActive(false);
    }
}
