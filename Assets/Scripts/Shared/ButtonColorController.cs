using System;
using System.Collections;
using System.Collections.Generic;
using Layer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ButtonColorController : MonoBehaviour
{
    [SerializeField] private MeshRenderer _buttonMeshRenderer;
    [SerializeField] private ButtonColorController buttonColorController;

    public Rigidbody _hingePointRigidBody;
    public Transform _hingePoint;
    public Color CurrentColor;
    public UnityEvent<bool> OnButtonColorChanged;
    

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerHelper.GetLayer(Layers.Player))
            ChangeColorToGreen();
    }

    public void ChangeColorToGreen()
    {
        if (CurrentColor != Color.green)
        {
            GameManager.Instance.TouchedButtons.Add(buttonColorController); 
            _buttonMeshRenderer.material.color = Color.green;
            CurrentColor = Color.green;
            OnButtonColorChanged.Invoke(true);
        }
    }

    public void ChangeColorToRed()
    {
        _buttonMeshRenderer.material.color = Color.red;
        CurrentColor = Color.red;
    }
    
    public void ChangeColorToYellow()
    {
        if (CurrentColor != Color.yellow)
        {
            if (CurrentColor == Color.green)
                OnButtonColorChanged.Invoke(false);
            
            GameManager.Instance.ComboAttemptButtons.Add(buttonColorController);
            _buttonMeshRenderer.material.color = Color.yellow;
            CurrentColor = Color.yellow;
        }
    }
}
