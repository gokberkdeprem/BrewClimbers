using System;
using System.Collections;
using System.Collections.Generic;
using Layer;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Object = System.Object;

public class AttachManager : MonoBehaviour
{
    [SerializeField] private Rigidbody _handRb;
    [SerializeField] private Collider _handCollider;
    [SerializeField] public UnityEvent OnAttachStatusChanged;
    public bool _isAttached;
    public bool _canAttach = true;
    [SerializeField] private GameObject _circleIndicator;
    private bool isCanAttachStatusChanging;
    public GameObject AttachedObject;
     public UnityEvent<bool> OnHitTarget;
    [SerializeField] private PlayerController _playerController;

    private void Start()
    {
        _circleIndicator.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        _circleIndicator.transform.position =
            new Vector3(_handCollider.transform.position.x, _handCollider.transform.position.y, _circleIndicator.transform.position.z);
        
        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if(!isCanAttachStatusChanging)
                    StartCoroutine(AttachDelay());
                
                _isAttached = false;
                OnAttachStatusChanged.Invoke();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerHelper.GetLayer(Layers.Brick) && _canAttach)
        {
            if (_playerController?.TargetObject != null)
            {
                var name = gameObject.name;

                var first = other.gameObject;
                var second = _playerController.TargetObject;

                var x = ReferenceEquals(first, second);
                
                OnHitTarget.Invoke(x);
                // _playerController.TargetObject = null;
            }
            
            AttachedObject = other.gameObject;
            _handRb.isKinematic = true;
            _isAttached = true;
            OnAttachStatusChanged.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerHelper.GetLayer(Layers.Brick))
        {
            AttachedObject = null;
            _handRb.isKinematic = false;
            _isAttached = false;
            OnAttachStatusChanged.Invoke();
        }
    }

    IEnumerator AttachDelay()
    {
        _canAttach = false;
        OnAttachStatusChanged.Invoke();
        isCanAttachStatusChanging = true;
        ToggleOrb();
        yield return new WaitForSeconds(0.5f);
        isCanAttachStatusChanging = false;
        _canAttach = true;
        OnAttachStatusChanged.Invoke();
        ToggleOrb();
    }

    private void ToggleOrb()
    {
        _circleIndicator.SetActive(!_circleIndicator.activeInHierarchy);
    }
}
