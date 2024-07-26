using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class HeightIndicatorController : MonoBehaviour
{
    [SerializeField] private Slider _heightSlider;
    [SerializeField] private Transform _finishPosition;
    [SerializeField]private Transform _groundPosition;
    private PlayerController _playerController;
    private bool _isPlayerInstantiated;
    
    
    private void Start()
    {
        GameManager.Instance.OnPlayerInstantiated.AddListener(OnPlayerInstantiated);
    }


    private void Update()
    {
        if (_isPlayerInstantiated)
        {
            var totalDistance = _finishPosition.position.y - _groundPosition.position.y;
        
            var doneDistance = _playerController.PlayerTransform.position.y - _groundPosition.position.y;
            var calculation = doneDistance / totalDistance;
        
            _heightSlider.value = calculation;
        }
    }

    private void OnPlayerInstantiated()
    {
        _playerController = GameManager.Instance.PlayerController;
        _isPlayerInstantiated = true;
    }

}
