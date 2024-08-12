using System;
using System.Collections;
using System.Collections.Generic;
using Layer;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using Object = System.Object;

public class AttachManager : MonoBehaviour
{
    [SerializeField] private Rigidbody _handRb;
    [SerializeField] private Rigidbody _elbowRb;
    [SerializeField] private Collider _handCollider;
    [SerializeField] public UnityEvent OnAttachStatusChanged;
    public bool _isAttached;
    public bool _canAttach = true;
    [SerializeField] private GameObject _circleIndicator;
    private bool isCanAttachStatusChanging;
    public GameObject AttachedObject;
     public UnityEvent<bool> OnHitTarget;
    [FormerlySerializedAs("_playerController")] [SerializeField] private PlayerControllerTJ playerControllerTj;
    [SerializeField] private GameObject _playerRoot;
    [SerializeField] private Rigidbody[] _rbs;
    [SerializeField] private bool _isTriggerEntered = false;

    private void Start()
    {
        _circleIndicator.SetActive(true);
        _rbs = _playerRoot.GetComponentsInChildren<Rigidbody>();
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

                _isTriggerEntered = false;
                _isAttached = false;
                OnAttachStatusChanged.Invoke();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerHelper.GetLayer(Layers.Button) && _canAttach && !_isTriggerEntered)
        {
            if (playerControllerTj?.TargetObject != null)
            {

                var first = other.gameObject;
                var second = playerControllerTj.TargetObject;

                var x = ReferenceEquals(first, second);
                OnHitTarget.Invoke(x);
            }
            
            other.gameObject.GetComponent<ButtonColorController>().ChangeColorToGreen();
            AttachedObject = other.gameObject;
            
            _handRb.isKinematic = true;
            _isAttached = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerHelper.GetLayer(Layers.Button))
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
        isCanAttachStatusChanging = true;
        ToggleCircleIndicator();
        yield return new WaitForSeconds(0.2f);
        isCanAttachStatusChanging = false;
        _canAttach = true;
        ToggleCircleIndicator();
    }

    private void ToggleCircleIndicator()
    {
        _circleIndicator.SetActive(!_circleIndicator.activeInHierarchy);
    }
}
