using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class AttachManager : MonoBehaviour
{
    [SerializeField] private Rigidbody _handRb;
    public bool _isAttached;
    private bool _canAttach = true;
    
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            // Get the first touch
            Touch touch = Input.GetTouch(0);

            // Check if the touch just began
            if (touch.phase == TouchPhase.Began)
            {
                StartCoroutine(AttachmentController());
                _handRb.isKinematic = false;
                _isAttached = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6 && _canAttach)
        {
            _handRb.isKinematic = true;
            _isAttached = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            _handRb.isKinematic = false;
            _isAttached = false;
        }
    }

    IEnumerator AttachmentController()
    {
        _canAttach = false;
        yield return new WaitForSeconds(0.2f);
        _canAttach = true;
    }
}
