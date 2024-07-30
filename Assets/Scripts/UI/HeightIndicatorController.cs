using Shared;
using UnityEngine;
using UnityEngine.UI;

public class HeightIndicatorController : MonoBehaviour
{
    [SerializeField] private Slider _heightSlider;
    [SerializeField] private Transform _finishPosition;
    [SerializeField]private Transform _groundPosition;
    private PlayerControllerTJ _playerControllerTj;
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
        
            var doneDistance = _playerControllerTj.PlayerTransform.position.y - _groundPosition.position.y;
            var calculation = doneDistance / totalDistance;
        
            _heightSlider.value = calculation;
        }
    }

    private void OnPlayerInstantiated()
    {
        _playerControllerTj = (PlayerControllerTJ)GameManager.Instance.PlayerController;
        _isPlayerInstantiated = true;
    }

}
