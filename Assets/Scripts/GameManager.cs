using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public bool IsGameOver;
    public UnityEvent OnGameOver;
    [SerializeField] private FinishLineController _finishLineController;
    
    // Start is called before the first frame update
    void Start()
    {
        IsGameOver = false;
        _finishLineController.OnFinish.AddListener(GameOver);
    }

    void GameOver()
    {
        IsGameOver = true;
        OnGameOver.Invoke();
    }
}
