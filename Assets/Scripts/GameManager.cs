using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public bool IsGameOver;
    public UnityEvent OnGameOver;
    public UnityEvent OnPlayerInstantiated;
    public CinemachineVirtualCamera VirtualCamera;
    [SerializeField] private FinishLineController _finishLineController;
    private PlayerController _playerController;
    public SpawnManager SpawnManager;
    public GroundController GroundController;
    
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    GameObject gameObject = new GameObject("GameManager");
                    _instance = gameObject.AddComponent<GameManager>();
                    // DontDestroyOnLoad(gameObject);
                }
            }
            return _instance;
        }
    }
    
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        // else if (_instance != this)
        // {
        //     Destroy(gameObject);
        // }
    }
    
    void Start()
    {
        IsGameOver = false;
        _finishLineController.OnFinish.AddListener(GameOver);
    }
    
    public PlayerController PlayerController
    {
        get => _playerController;
        set
        {
            _playerController = value;
            OnPlayerInstantiated.Invoke();
        }
    }
    
    void GameOver()
    {
        IsGameOver = true;
        OnGameOver.Invoke();
    }
}
