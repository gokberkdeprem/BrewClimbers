using System;
using System.Collections;
using System.Collections.Generic;
using Layer;
using UnityEngine;
using UnityEngine.Events;

public class GroundController : MonoBehaviour
{
    public UnityEvent OnGroundTouched;


    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.layer == LayerHelper.GetLayer(Layers.Player))
            OnGroundTouched.Invoke();
    }
}
