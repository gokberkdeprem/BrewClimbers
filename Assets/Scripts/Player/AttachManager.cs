using System;
using System.Collections;
using System.Collections.Generic;
using Layer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class AttachManager : MonoBehaviour
{
    [SerializeField] private Rigidbody _handRb;
    public bool _isAttached;
    private bool _canAttach = true;

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
                StartCoroutine(AttachDelay());
                _handRb.isKinematic = false;
                _isAttached = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerHelper.GetLayer(Layers.Brick) && _canAttach)
        {
            _handRb.isKinematic = true;
            _isAttached = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerHelper.GetLayer(Layers.Brick))
        {
            _handRb.isKinematic = false;
            _isAttached = false;
        }
    }

    IEnumerator AttachDelay()
    {
        _canAttach = false;
        yield return new WaitForSeconds(0.3f);
        _canAttach = true;
    }
}
