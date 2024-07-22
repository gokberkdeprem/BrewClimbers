using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _hips;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private ThresholdController _thresholdController;
    [SerializeField] private Button _respawnButton;
    [SerializeField] private SinglePlayerUIManager singlePlayerUIManager;
    
    void Start()
    {
        _respawnButton.onClick.AddListener(MoveToSpawnPoint);
        _player = Instantiate(_playerPrefab, _spawnPoint.position, _spawnPoint.rotation);
        _hips = _player.transform.GetChild(1).transform.GetChild(0);
        MoveToSpawnPoint();
        _thresholdController.OnFall.AddListener(MoveToSpawnPoint);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _hips.transform.position = _spawnPoint.position;
        }
    }

    private void MoveToSpawnPoint()
    {
        _hips.transform.position = _spawnPoint.position;
    }
    
}
