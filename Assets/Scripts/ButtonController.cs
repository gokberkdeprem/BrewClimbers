using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private MeshRenderer _buttonMeshRenderer;
    [SerializeField] private ButtonController _buttonController;
    public Color CurrentColor;
    
    public void ChangeColorToGreen()
    {
        if (CurrentColor != Color.green)
        {
            GameManager.Instance.TouchedButtons.Add(_buttonController); 
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
            GameManager.Instance.ComboAttemptButtons.Add(_buttonController);
            _buttonMeshRenderer.material.color = Color.yellow;
            CurrentColor = Color.yellow;
        }
    }
}
