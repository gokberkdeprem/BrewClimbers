using System;
using System.Collections;
using System.Collections.Generic;
using Layer;
using UnityEngine;
using UnityEngine.Events;

public class ThresholdController : MonoBehaviour
{
    public UnityEvent OnFall;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerHelper.GetLayer(Layers.Player))
        {
            OnFall.Invoke();
        }
    }
}
