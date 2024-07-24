using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Cinemachine;
using Layer;
using TMPro;
using UI;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public UnityEvent<Transform> OnTouch;
    public UnityEvent<float> OnStaminaChanged;
    [SerializeField] private Rigidbody _rightHandRb;
    [SerializeField] private Rigidbody _leftHandRb;
    [SerializeField] private Rigidbody _hipsRb;
    [SerializeField] private float _force;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private int _staminaLimit;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private bool _isAttached;
    [SerializeField] private bool _canAttach;

    [SerializeField] private AttachManager _rightHandAttMan;
    [SerializeField] private AttachManager _leftHandAttMan;
    
    
    [SerializeField] private float _stamina;
    [SerializeField] private float _staminaFillRate;
    private GameManager _gameManager;
    private bool isFilling;
    private GameObject _tappedObject;

    // public int RemainingTap => _remainingTap;
    public float Stamina
    {
        get => _stamina;
        private set
        {
            _stamina = value;

            if (_stamina < 0 || _stamina > _staminaLimit)
            {
                if (_stamina > _staminaLimit)
                {
                    _stamina = _staminaLimit;
                }
                else if (_stamina < 0)
                {
                    _stamina = 0;
                }
            }
        }
    }

    private bool CanJump => Stamina >= 1;


    [SerializeField] private Rigidbody[] rigidbodies;
    // [ContextMenu("Set Rigidbodies")]
    // void SetRigidBodies()
    // {
    //     rigidbodies = GetComponentsInChildren<Rigidbody>();
    // }
    
    
    void Start()
    {
        _gameManager = GameManager.Instance;
        _virtualCamera = GameManager.Instance.VirtualCamera;
        _stamina = _staminaLimit;
        AssignCamera();
        _gameManager.OnGameOver.AddListener(GameOver);
        _gameManager.PlayerController = _playerController;
        StartCoroutine(CheckAttachStatus());
        _gameManager.SpawnManager.OnRespawn.AddListener(OnRespawn);
    }

    private void OnRespawn()
    {
        Stamina = _staminaLimit;
    }

    private IEnumerator CheckAttachStatus()
    {
        while (!_gameManager.IsGameOver)
        {
            yield return new WaitForSeconds(1f);
            
            if (_isAttached && !isFilling)
            {
                StartCoroutine(FillStamina());
            }
        }
    }

    private IEnumerator FillStamina()
    {
        isFilling = true;
        while (_isAttached && Stamina < 3 && _staminaFillRate > 0)
        {
            yield return new WaitForSeconds(0.01f);
            Stamina += _staminaFillRate;
        }
        isFilling = false;
    }

    // Update is called once per frame
    void Update()
    {
        _isAttached = _leftHandAttMan._isAttached || _rightHandAttMan._isAttached;
        _canAttach = _leftHandAttMan._canAttach && _rightHandAttMan._canAttach;
        
        if (Input.touchCount > 0 && !_gameManager.IsGameOver && CanJump)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    var colliderGameObject = hit.collider.gameObject;
                    
                    if (colliderGameObject.layer == LayerHelper.GetLayer(Layers.Brick) && colliderGameObject != _tappedObject)
                    {
                        _tappedObject = colliderGameObject;
                        
                        OnTouch.Invoke(colliderGameObject.transform);
                        Vector3 target = hit.collider.gameObject.transform.position;
                        var activeHand = target.x > transform.position.x ? _rightHandRb : _leftHandRb;
                        _rightHandRb.isKinematic = false;
                        _leftHandRb.isKinematic = false;
                        
                        Vector3 targetDirection = (target - activeHand.transform.position).normalized;
                        activeHand.AddForce(targetDirection * _force, ForceMode.Impulse);
                        
                        Stamina -= 1;
                    }

                    StartCoroutine(RemoveTappedObject());

                }
            }
        }
    }

    private IEnumerator RemoveTappedObject()
    {
        // To prevent the double tap
        yield return new WaitForSeconds(1f);
        _tappedObject = null;
    }

    // private IEnumerator JumpCoolDown()
    // {
    //     yield return new WaitForSeconds(_jumpCoolDownDuration);
    //     OnTapCountChange.Invoke(3, true);
    //     _remainingTap = _tapLimit;
    // }

    private void AssignCamera()
    {
        _virtualCamera.LookAt = _hipsRb.transform;
        _virtualCamera.Follow = _hipsRb.transform;
    }

    private void GameOver()
    {
        foreach (var rb in rigidbodies)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

}
