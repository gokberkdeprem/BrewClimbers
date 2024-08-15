using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public List<CinemachineVirtualCamera> CameraQueue;
    [SerializeField] private CinemachineVirtualCamera _vCam;
    private List<GameObject> _players;
    private int _cameraIndex = 0;
    
    private static CameraController _instance;
    
    public static CameraController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CameraController>();

                if (_instance == null)
                {
                    GameObject gameObject = new GameObject("GameManager");
                    _instance = gameObject.AddComponent<CameraController>();
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
        }
        
    }

    public GameObject Players
    {
        get => _players.LastOrDefault();
        set
        {
            _vCam.m_LookAt = value.transform;
            _vCam.m_Follow = value.transform;
            _cameraIndex += 1;
            _players.Add(value);
        }
    }
}
