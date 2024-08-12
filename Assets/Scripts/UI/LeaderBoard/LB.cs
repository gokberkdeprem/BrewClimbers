using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LB : MonoBehaviour
{
    public UnityEvent OnEnabled;
    public void OnEnable()
    {
        OnEnabled.Invoke();
    }
}
