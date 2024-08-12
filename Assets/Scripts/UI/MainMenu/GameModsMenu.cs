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

    private void Start()
    {
        _tapJumpStartButton.onClick.AddListener(delegate{LoadGameScene(GameModScene.TapJump);});
        _ropeSwingStartButton.onClick.AddListener(delegate{LoadGameScene(GameModScene.RopeSwing);});
    }


    private void LoadGameScene(GameModScene scene)
    {
        SceneManager.LoadScene((int)scene);
    }
}
