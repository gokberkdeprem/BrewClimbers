using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rightHandRb;
    [SerializeField] private Rigidbody _leftHandRb;
    [SerializeField] private Rigidbody _hipsRb;
    [SerializeField] private float _force;

    [SerializeField] private AttachManager _attachManager;
    
    // Start is called before the first frame update
    private Rigidbody[] rigidbodies;

    void Start()
    {
        //
        // // Get all rigidbodies attached to the character
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.layer == 6)
                    {
                        Vector3 target = hit.collider.gameObject.transform.position;
                        var activeHand = target.x > transform.position.x ? _rightHandRb : _leftHandRb;
                        
                        Vector3 targetDirection = (target - activeHand.transform.position).normalized;
                        activeHand.AddForce(targetDirection * _force, ForceMode.Impulse);
                    }
                }
            }
        }
    }

}
