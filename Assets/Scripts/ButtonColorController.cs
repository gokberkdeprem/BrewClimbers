using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ButtonColorController : MonoBehaviour
{
    [SerializeField] private MeshRenderer _buttonMeshRenderer;
    [SerializeField] private ButtonColorController buttonColorController;
    public Transform _hingePoint;
    public Color CurrentColor;
    
    public void ChangeColorToGreen()
    {
        if (CurrentColor != Color.green)
        {
            GameManager.Instance.TouchedButtons.Add(buttonColorController); 
            _buttonMeshRenderer.material.color = Color.green;
            CurrentColor = Color.green;
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
            GameManager.Instance.ComboAttemptButtons.Add(buttonColorController);
            _buttonMeshRenderer.material.color = Color.yellow;
            CurrentColor = Color.yellow;
        }
    }
}
