using System;
using System.Collections;
using Cinemachine;
using Layer;
using Shared;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace RopeSwing
{
    public class PlayerControllerRS : PlayerController
    {
        [SerializeField] private Rigidbody _rightHandRb;
        [SerializeField] private Rigidbody _leftHandRb;
        
        [SerializeField] private Rigidbody _hipsRb;
        [SerializeField] private float _force;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private int _staminaLimit;
        [SerializeField] private PlayerControllerRS playerControllerRS;
        [SerializeField] private Transform _ropeStart;
        [SerializeField] private Transform _ropeEnd;
        
        [SerializeField] private float _ropeLength = 3;
        private GameManager _gameManager;
    
        private bool isFilling;
    
        private GameObject _tappedObject;
        public GameObject TargetObject;
        public ButtonColorController targetObjectColorController;
        private int _combo;
        private bool isTapped;
        private Rigidbody _activeHand;
        public UnityEvent _killCoRoutine;
        private Coroutine currentCoRoutine;
        

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
        void Update()
        {
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
                            if (currentCoRoutine != null)
                                StopCoroutine(currentCoRoutine);
                            
                            TargetObject = colliderGameObject;

                            targetObjectColorController = TargetObject.GetComponent<ButtonColorController>();
                            targetObjectColorController.ChangeColorToYellow();
                        
                            // Instantiates touch light effect.
                            OnTouch.Invoke(colliderGameObject.transform);
                            
                            Vector3 target = hit.transform.position;
                        
                            _activeHand = target.x > transform.position.x ? _rightHandRb : _leftHandRb;
                            _rightHandRb.isKinematic = false;
                            _leftHandRb.isKinematic = false;

                            _ropeEnd.position = targetObjectColorController._hingePoint.transform.position;

                            currentCoRoutine = StartCoroutine(MergeSpheres());
                        }
                    }
                }
            }
        }

        private IEnumerator MergeSpheres()
        {
            while (true)
            {
                var distance = Vector3.Distance(_ropeStart.position, _ropeEnd.position);
                _ropeStart.position = _activeHand.transform.position;
                var isDownward = _ropeStart.position.y > _ropeEnd.position.y;
                
                if(distance >= _ropeLength)
                    _activeHand.AddForce((_ropeEnd.position - _activeHand.position).normalized * (_force * distance / (isDownward ? 6 : 4)), ForceMode.Force);
                
                yield return new WaitForFixedUpdate();
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
