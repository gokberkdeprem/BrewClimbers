using System;
using System.Collections;
using System.Collections.Generic;
using Layer;
using UnityEngine;
using UnityEngine.Events;

public class FinishLineController : MonoBehaviour
{
    public UnityEvent OnFinish;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerHelper.GetLayer(Layers.Player))
        {
            OnFinish.Invoke();
        }
    }
}
