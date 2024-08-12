using System;
using System.Collections;
using System.Collections.Generic;
using Layer;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerAnimationController _pac;

    private void Start()
    {
        _animator.Play("ArmStretching");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && !GameManager.Instance.IsGameOver)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    var colliderGameObject = hit.collider.gameObject;
                    
                    if (colliderGameObject.layer == LayerHelper.GetLayer(Layers.Button))
                    {
                        _animator.enabled = false;
                        _pac.enabled = false;
                    }
                }
            }
        }
    }
}
