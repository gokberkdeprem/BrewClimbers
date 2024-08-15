using System;
using System.Collections;
using Cinemachine;
using Fusion;
using Layer;
using Shared;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

namespace RopeSwing
{
    public class MultiPlayerControllerRS : NetworkBehaviour
    {
        [SerializeField] private MultiPlayerControllerRS _script;
        [SerializeField] private Rigidbody _rightHandRb;
        [SerializeField] private Rigidbody _leftHandRb;
        [SerializeField] private SpringJoint _ropeEndSpringJoint;
        [SerializeField] private float _currentRopeLength;
        
        [SerializeField] private Rigidbody _hipsRb;
        [SerializeField] private float _force;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private int _staminaLimit;
        [SerializeField] private PlayerControllerRS playerControllerRS;
        [SerializeField] private Transform _ropeStart;
        [SerializeField] private Transform _ropeEnd;
        
        [SerializeField] private float _ropeLengthMin = 3;
        [SerializeField] private float _ropeLengthMax = 3;
        private GameManager _gameManager;
    
        private bool isFilling;
    
        private GameObject _tappedObject;
        public GameObject TargetObject;
        public ButtonColorController targetObjectColorController;
        private int _combo;
        private bool isTapped;
        private Rigidbody _activeHand;
        public UnityEvent _killCoRoutine;
        private bool currentCoRoutine;


        private void Awake()
        {
            if (SystemInfo.deviceType != DeviceType.Handheld)
                _script.enabled = false;
        }

        public float CurrentRopeLength
        {
            get => _currentRopeLength;
            set
            {
                _currentRopeLength = value;
                if (_currentRopeLength < 0)
                    _currentRopeLength = 0;
            }
        }

        [SerializeField] private Rigidbody[] rigidbodies;
        [ContextMenu("Enable Projections")]
        void SetRigidBodies()
        {
            rigidbodies = GetComponentsInChildren<Rigidbody>();
            // CharacterJoint[] characterJoints = GetComponentsInChildren<CharacterJoint>();
            // foreach (var cj in characterJoints)
            // {
            //     
            // }

            foreach (var rb in rigidbodies)
            {
                rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            }
        
        }
        void Start()
        {
            _gameManager = GameManager.Instance;
            _virtualCamera = GameManager.Instance.VirtualCamera;
                AssignCamera();
            _gameManager.OnGameOver.AddListener(GameOver);
            _gameManager.PlayerController = playerControllerRS;
        
        }
        
        // Update is called once per frame
        public override void FixedUpdateNetwork()
        {
            // Only move own player and not every other player. Each player controls its own player object.
            if (HasStateAuthority == false)
            {
                return;
            }
            
            if (Input.touchCount > 0 && !_gameManager.IsGameOver)
            {
                Touch touch = Input.GetTouch(0);
            
                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        var colliderGameObject = hit.collider.gameObject;
                    
                        if(TargetObject && _activeHand)
                            _killCoRoutine.Invoke();
                    
                    
                        if (colliderGameObject.layer == LayerHelper.GetLayer(Layers.Button))
                        {
                            currentCoRoutine = false;
                            
                            TargetObject = colliderGameObject;

                            targetObjectColorController = TargetObject.GetComponent<ButtonColorController>();
                            targetObjectColorController.ChangeColorToYellow();
                        
                            // Instantiates touch light effect.
                            // OnTouch.Invoke(colliderGameObject.transform);
                            
                            Vector3 target = hit.transform.position;
                        
                            _activeHand = target.x > transform.position.x ? _rightHandRb : _leftHandRb;
                            _rightHandRb.isKinematic = false;
                            _leftHandRb.isKinematic = false;

                            _ropeEnd.position = targetObjectColorController._hingePoint.transform.position;
                            
                            CurrentRopeLength = _ropeLengthMax;
                            currentCoRoutine = true;
                        }
                    }
                }
                
                if (currentCoRoutine)
                {
                    var distance = Vector3.Distance(_ropeStart.position, _ropeEnd.position);
                    _ropeStart.position = _activeHand.transform.position;
                    var isDownward = _ropeStart.position.y > _ropeEnd.position.y;
                
                    if (distance >= _ropeLengthMin && !isDownward)
                    {
                        _activeHand.AddForce((_ropeEnd.position - _activeHand.position).normalized * _force , ForceMode.Acceleration);
                    }

                    if (distance >= CurrentRopeLength)
                    {
                        _activeHand.velocity = Vector3.zero;
                        _activeHand.angularVelocity = Vector3.zero; 
                    }

                    if (CurrentRopeLength > 0)
                    {
                        CurrentRopeLength -= 0.1f;
                    }
                }
                
            }
        }

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
}
