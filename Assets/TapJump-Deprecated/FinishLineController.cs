using System;
using System.Collections;
using System.Collections.Generic;
using Layer;
using UnityEngine;
using UnityEngine.Events;

public class FinishLineController : MonoBehaviour
{
    public UnityEvent OnFinish;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerHelper.GetLayer(Layers.Player))
        {
            OnFinish.Invoke();
        }
    }
}
