using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Cinemachine;
using Layer;
using TMPro;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public UnityEvent<Transform> OnTouch;
    public UnityEvent<int, bool> OnTapCountChange;
    [SerializeField] private Rigidbody _rightHandRb;
    [SerializeField] private Rigidbody _leftHandRb;
    [SerializeField] private Rigidbody _hipsRb;
    [SerializeField] private float _force;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private float _jumpCoolDownDuration;
    [SerializeField] private int _tapLimit;
    [SerializeField] private PlayerController _playerController;
    
    private int _remainingTap;
    private GameManager _gameManager;
    private bool _canJump = true;

    public int RemainingTap => _remainingTap;

    [SerializeField] private Rigidbody[] rigidbodies;
    [ContextMenu("Set Rigidbodies")]
    void SetRigidBodies()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
    }
    
    void Start()
    {
        _gameManager = GameManager.Instance;
        _virtualCamera = GameManager.Instance.VirtualCamera;
        _remainingTap = _tapLimit;
        AssignCamera();
        _gameManager.OnGameOver.AddListener(GameOver);
        _gameManager.PlayerController = _playerController;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && !_gameManager.IsGameOver && _canJump)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    var colliderGameObject = hit.collider.gameObject;
                    
                    if (colliderGameObject.layer == LayerHelper.GetLayer(Layers.Brick))
                    {
                        OnTouch.Invoke(colliderGameObject.transform);
                        Vector3 target = hit.collider.gameObject.transform.position;
                        var activeHand = target.x > transform.position.x ? _rightHandRb : _leftHandRb;
                        Vector3 targetDirection = (target - activeHand.transform.position).normalized;
                        activeHand.AddForce(targetDirection * _force, ForceMode.Impulse);
                        
                        _remainingTap -= 1;
                        OnTapCountChange.Invoke(_remainingTap, false);
                        if (_remainingTap <= 0)
                        {
                            StartCoroutine(JumpCoolDown());
                        }
                    }
                }
            }
        }
    }

    private IEnumerator JumpCoolDown()
    {
        _canJump = false;
        yield return new WaitForSeconds(_jumpCoolDownDuration);
        _canJump = true;
        _remainingTap = _tapLimit;
        OnTapCountChange.Invoke(_remainingTap, true);
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
