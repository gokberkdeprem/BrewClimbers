using System;
using System.Collections;
using System.Collections.Generic;
using Layer;
using UnityEngine;
using UnityEngine.Events;

public class GroundController : MonoBehaviour
{
    public UnityEvent OnGroundTouched;
    private bool _canTouch = true;


    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.layer == LayerHelper.GetLayer(Layers.Player) && _canTouch)
            OnGroundTouched.Invoke();
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == LayerHelper.GetLayer(Layers.Player))
            StartCoroutine(DisableGround());
    }

    private IEnumerator DisableGround()
    {
        _canTouch = false;
        yield return new WaitForSeconds(0.5f);
        _canTouch = true;
    }
}
