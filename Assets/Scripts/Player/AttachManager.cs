using System;
using System.Collections;
using System.Collections.Generic;
using Layer;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class AttachManager : MonoBehaviour
{
    [SerializeField] private Rigidbody _handRb;
    [SerializeField] private Collider _handCollider;
    [SerializeField] public UnityEvent OnAttachStatusChanged;
    public bool _isAttached;
    public bool _canAttach = true;
    [SerializeField] private GameObject _greenOrbParticle;
    private bool isCanAttachStatusChanging;

    private void Start()
    {
        _greenOrbParticle.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        _greenOrbParticle.transform.position =
            new Vector3(_handCollider.transform.position.x, _handCollider.transform.position.y, _greenOrbParticle.transform.position.z);
        
        
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
            _handRb.isKinematic = true;
            _isAttached = true;
            OnAttachStatusChanged.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerHelper.GetLayer(Layers.Brick))
        {
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
        _greenOrbParticle.SetActive(!_greenOrbParticle.activeInHierarchy);
    }
    
    
    
}
