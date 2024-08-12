using System.Collections;
using DG.Tweening;
using Shared;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StaminaSlider : MonoBehaviour
    {
        [SerializeField] private Slider _staminaSlider;
        [SerializeField] private Image[] _staminaJumpCounts = new Image[3];
        [SerializeField] private Sprite _goldColor;
        [SerializeField] private Sprite _greyColor;

        [SerializeField] private Image _staminaFillColor;
        [SerializeField] private Sprite _redSliderFill;
        [SerializeField] private Sprite _goldSliderFill;
        
        
        // [SerializeField] private float _fillDuration = 3;
        [SerializeField] private float _emptyDuration = 0.1f;
        [SerializeField] private float _value;
        
        private PlayerControllerTJ _playerControllerTj;
        private Sequence _sequence;
        private bool canUpdateStamina;
        
        private void Start()
        {
            GameManager.Instance.OnPlayerInstantiated.AddListener(OnPlayerInstantiated);
            
            _staminaSlider.value = 0;
        }

        private void Update()
        {
            if (canUpdateStamina)
            {
                _staminaSlider.value = _playerControllerTj.Stamina;
            
                if (Mathf.Approximately(_playerControllerTj.Stamina, 3))
                {
                    _staminaJumpCounts[0].sprite = _goldColor;
                    _staminaJumpCounts[1].sprite = _goldColor;
                    _staminaJumpCounts[2].sprite = _goldColor;
                }
                else if (_playerControllerTj.Stamina >= 2)
                {
                    _staminaJumpCounts[0].sprite = _goldColor;
                    _staminaJumpCounts[1].sprite = _goldColor;
                    _staminaJumpCounts[2].sprite = _greyColor;
                }
                else if (_playerControllerTj.Stamina >= 1)
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
        }

        private void OnPlayerInstantiated()
        {
            _playerControllerTj = (PlayerControllerTJ)GameManager.Instance.PlayerController;
            canUpdateStamina = true;
            _playerControllerTj.OnInsufficientStamina.AddListener(ChangeSliderFillColor);
        }

        private void ChangeSliderFillColor()
        {
            StartCoroutine(ToggleColor());
        }

        private IEnumerator ToggleColor()
        {
            _staminaFillColor.sprite = _redSliderFill;
            yield return new WaitForSeconds(0.5f);
            _staminaFillColor.sprite = _goldSliderFill;
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