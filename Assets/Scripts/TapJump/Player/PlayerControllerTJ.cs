using System;
using System.Collections;
using Cinemachine;
using Layer;
using Shared;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerControllerTJ : PlayerController
{
    public UnityEvent OnInsufficientStamina;
    public UnityEvent<int> OnStreakChanged;
    public UnityEvent OnJumpStart;
    public Transform PlayerTransform;
    public bool _isJumping;
    
    
    [SerializeField] private Rigidbody _rightHandRb;
    [SerializeField] private Rigidbody _leftHandRb;
    [SerializeField] private Rigidbody _hipsRb;
    [SerializeField] private float _force;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private int _staminaLimit;
    [FormerlySerializedAs("_playerController")] [SerializeField] private PlayerControllerTJ playerControllerTj;
    [SerializeField] private bool _isAttached;

    [SerializeField] private AttachManager _rightHandAttMan;
    [SerializeField] private AttachManager _leftHandAttMan;
    
    [SerializeField] private float _stamina;
    [SerializeField] private float _staminaFillRate;
    private GameManager _gameManager;
    
    private bool isFilling;
    
    private GameObject _tappedObject;
    public GameObject TargetObject;
    [FormerlySerializedAs("TargetObjectController")] public ButtonColorController targetObjectColorController;
    private int _combo;

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

    public int Combo
    {
        get => _combo;
        set
        {
            _combo = value;
            OnStreakChanged.Invoke(_combo);
        }
    }
    

    private bool CanJump
    {
        get
        {
            if (Stamina >= 1)
            {
                return true;
            }
            OnInsufficientStamina.Invoke(); 
            return false;
        }
    }

    [SerializeField] private Rigidbody[] rigidbodies;
    [ContextMenu("Enable Projections")]
    void SetRigidBodies()
    {
        // rigidbodies = GetComponentsInChildren<Rigidbody>();
        CharacterJoint[] characterJoints = GetComponentsInChildren<CharacterJoint>();
        foreach (var cj in characterJoints)
        {
            cj.enableProjection = true;
        }
    }
    
    
    void Start()
    {
        if (_staminaFillRate <= 0)
        {
            _staminaFillRate = 1;
            throw new Exception("Stamina fill rate must be greater than 0 to prevent infinite loop at FillStamina method.");
        }
        
        _gameManager = GameManager.Instance;
        _virtualCamera = GameManager.Instance.VirtualCamera;
        _stamina = _staminaLimit;
        AssignCamera();
        _gameManager.OnGameOver.AddListener(GameOver);
        _gameManager.PlayerController = playerControllerTj;
        StartCoroutine(CheckAttachStatus());
        _gameManager.SpawnManager.OnRespawn.AddListener(ResetPlayer);
        _gameManager.GroundController.OnGroundTouched.AddListener(ResetPlayer);
        
        _rightHandAttMan.OnHitTarget.AddListener(CheckForCombo);
        _leftHandAttMan.OnHitTarget.AddListener(CheckForCombo);
        
        _rightHandAttMan.OnAttachStatusChanged.AddListener(delegate{OnJumpStart.Invoke();});
        _leftHandAttMan.OnAttachStatusChanged.AddListener(delegate{OnJumpStart.Invoke();});
    }

    private void ResetPlayer()
    {
        Stamina = _staminaLimit;
        Combo = 0;
        _gameManager.ResetButtonColors();
        _gameManager.ResetComboAttemptButtons();
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
    
    private void CheckForCombo(bool isHitToTarget)
    {
        if (isHitToTarget)
        {
            targetObjectColorController.ChangeColorToGreen();
            GameManager.Instance.ResetComboAttemptButtons();
            Stamina += 1;
            Combo += 1;
            _rightHandAttMan.AttachedObject = null;
            _leftHandAttMan.AttachedObject = null;
        }
        else
        {
            GameManager.Instance.ResetComboAttemptButtons();
            Combo = 0;
        }
        TargetObject = null;
    }
    
    // private IEnumerator CheckForStreak()
    // {
    //     yield return new WaitForSeconds(1.5f);
    //     
    //     if (TargetObject == _rightHandAttMan.AttachedObject || TargetObject == _leftHandAttMan.AttachedObject)
    //     {
    //         _rightHandAttMan.AttachedObject = null;
    //         _leftHandAttMan.AttachedObject = null;
    //         Stamina += 1;
    //         Combo += 1;
    //     }
    //     else
    //     {
    //         Combo = 0;
    //     }
    // }
    
    private IEnumerator FillStamina()
    {
        isFilling = true;
        while (_isAttached && Stamina < 3 && _staminaFillRate > 0)
        {
            yield return new WaitForSeconds(0.01f);
            Stamina += Time.deltaTime * _staminaFillRate;
        }
        isFilling = false;
    }

    // Update is called once per frame
    void Update()
    {
        _isAttached = _leftHandAttMan._isAttached || _rightHandAttMan._isAttached;
        
        _isJumping = !_isAttached;
        
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
                    
                    if (colliderGameObject.layer == LayerHelper.GetLayer(Layers.Button) && colliderGameObject != _tappedObject)
                    {
                        _tappedObject = colliderGameObject;
                        TargetObject = colliderGameObject;

                        targetObjectColorController = TargetObject.GetComponent<ButtonColorController>();
                        targetObjectColorController.ChangeColorToYellow();
                        
                        OnTouch.Invoke(colliderGameObject.transform);
                        // Vector3 target = hit.collider.gameObject.transform.position;
                        Vector3 target = hit.transform.position;
                        
                        
                        var activeHand = target.x > transform.position.x ? _rightHandRb : _leftHandRb;
                        _rightHandRb.isKinematic = false;
                        _leftHandRb.isKinematic = false;
                        
                        Vector3 targetDirection = (target - activeHand.transform.position).normalized;
                        activeHand.AddForce(targetDirection * _force, ForceMode.Impulse);
                        Stamina -= 1;

                        if (!_isJumping)
                        {
                            _isJumping = true;
                            OnJumpStart.Invoke();
                        }
                    }

                    StartCoroutine(RemoveTappedObject());
                }
            }
        }
    }

    private IEnumerator RemoveTappedObject()
    {
        // To prevent the double tap to a obstacle
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
