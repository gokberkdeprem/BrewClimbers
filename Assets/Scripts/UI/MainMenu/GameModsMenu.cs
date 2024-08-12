using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameModsMenu : MonoBehaviour
{
    [SerializeField] private Button _tapJumpStartButton;
    [SerializeField] private Button _ropeSwingStartButton;
    
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _hiddenCloseButton;
    [SerializeField] private GameObject _gameModsObject;

    private void Start()
    {
        _closeButton.onClick.AddListener(CloseSettings);
        _hiddenCloseButton.onClick.AddListener(CloseSettings);
        // _tapJumpStartButton.onClick.AddListener(delegate{LoadGameScene(GameModScene.TapJump);});
        _ropeSwingStartButton.onClick.AddListener(delegate{LoadGameScene(GameModScene.RopeSwing);});
    }


    private void LoadGameScene(GameModScene scene)
    {
        SceneManager.LoadScene((int)scene);
    }
    
    private void CloseSettings()
    {
        _gameModsObject.SetActive(false);
    }
}
