using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Cinemachine;
using Layer;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public UnityEvent<Transform> OnTouch;
    [SerializeField] private Rigidbody _rightHandRb;
    [SerializeField] private Rigidbody _leftHandRb;
    [SerializeField] private Rigidbody _hipsRb;
    [SerializeField] private float _force;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    private GameManager _gameManager;
    
    // Start is called before the first frame update
    private Rigidbody[] rigidbodies;

    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        _virtualCamera = GameObject.FindWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        AssignCamera();
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _gameManager.OnGameOver.AddListener(GameOver);
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
                    
                    if (colliderGameObject.layer == LayerHelper.GetLayer(Layers.Brick))
                    {
                        OnTouch.Invoke(colliderGameObject.transform);
                        Vector3 target = hit.collider.gameObject.transform.position;
                        var activeHand = target.x > transform.position.x ? _rightHandRb : _leftHandRb;
                        Vector3 targetDirection = (target - activeHand.transform.position).normalized;
                        activeHand.AddForce(targetDirection * _force, ForceMode.Impulse);
                    }
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
