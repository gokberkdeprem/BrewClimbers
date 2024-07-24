using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class StaminaSlider : MonoBehaviour
    {
        [SerializeField] private Slider _staminaSlider;
        [SerializeField] private Sprite _goldColor;
        [SerializeField] private Sprite _greyColor;
        [SerializeField] private float _fillDuration = 3;
        [SerializeField] private float _emptyDuration = 0.1f;
        [SerializeField] private Button testButton;
        [SerializeField] private float _value;
        [SerializeField] private Image[] _staminaJumpCounts = new Image[3];
        private Sequence _sequence;
        private PlayerController _playerController;
        private bool canUpdateStamina;
        private float _previousStamina;
        private void Start()
        {
            GameManager.Instance.OnPlayerInstantiated.AddListener(InitializeStamina);
            _staminaSlider.value = 0;
            _previousStamina = -1;
        }

        private void Update()
        {
            if(canUpdateStamina)
             _staminaSlider.value = _playerController.Stamina;
            
            if (Mathf.Approximately(_playerController.Stamina, 3))
            {
                _staminaJumpCounts[0].sprite = _goldColor;
                _staminaJumpCounts[1].sprite = _goldColor;
                _staminaJumpCounts[2].sprite = _goldColor;
            }
            else if (_playerController.Stamina >= 2)
            {
                _staminaJumpCounts[0].sprite = _goldColor;
                _staminaJumpCounts[1].sprite = _goldColor;
                _staminaJumpCounts[2].sprite = _greyColor;
            }
            else if (_playerController.Stamina >= 1)
            {
                _staminaJumpCounts[0].sprite = _goldColor;
                _staminaJumpCounts[1].sprite = _greyColor;
                _staminaJumpCounts[2].sprite = _greyColor;
            }
            else
            {
                _staminaJumpCounts[0].sprite = _greyColor;
                _staminaJumpCounts[1].sprite = _greyColor;
                _staminaJumpCounts[2].sprite = _greyColor;
            }
            
        }

        private void InitializeStamina()
        {
            _playerController = GameManager.Instance.PlayerController;
            canUpdateStamina = true;
        }

        public void FillSliderWithCustomValue(float targetValue, bool isFill, float fillDuration)
        {
            // Ensure the target value is within the slider's range
            targetValue = Mathf.Clamp(targetValue, _staminaSlider.minValue, _staminaSlider.maxValue);
            
            if (isFill)
            {
                if (!_sequence.IsActive())
                    _sequence = DOTween.Sequence();
                
                var currentTween = _staminaSlider.DOValue(targetValue, fillDuration).SetEase(Ease.Linear);
                _sequence.Append(currentTween);
            }
            else
            {
                _sequence.Kill();
                _staminaSlider.DOValue(targetValue, _emptyDuration).SetEase(Ease.Linear);
            }
        }
    }
}
// if (Mathf.Approximately(_playerController.Stamina, 3))
// {
//     _staminaJumpCounts[0].sprite = _goldColor;
//     _staminaJumpCounts[1].sprite = _goldColor;
//     _staminaJumpCounts[2].sprite = _goldColor;
// }
// else if (_playerController.Stamina >= 2)
// {
//     _staminaJumpCounts[0].sprite = _goldColor;
//     _staminaJumpCounts[1].sprite = _goldColor;
//     _staminaJumpCounts[2].sprite = _greyColor;
// }
// else if (_playerController.Stamina >= 1)
// {
//     _staminaJumpCounts[0].sprite = _goldColor;
//     _staminaJumpCounts[1].sprite = _greyColor;
//     _staminaJumpCounts[2].sprite = _greyColor;
// }
// else
// {
//     _staminaJumpCounts[0].sprite = _greyColor;
//     _staminaJumpCounts[1].sprite = _greyColor;
//     _staminaJumpCounts[2].sprite = _greyColor;
// }

// float currentStamina = _playerController.Stamina;
//
// if (!Mathf.Approximately(currentStamina, _previousStamina))
// {
//     _previousStamina = currentStamina;
//     int stamina = Mathf.CeilToInt(currentStamina);
//
//     // Update sprites based on the stamina level
//     for (int i = 0; i < _staminaJumpCounts.Length; i++)
//     {
//         _staminaJumpCounts[i].sprite = i <= stamina ? _goldColor : _greyColor;
//     }
// }