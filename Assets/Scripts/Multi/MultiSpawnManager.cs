using Fusion;
using UnityEngine;

public class MultiSpawnManager : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject _playerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Runner.Spawn(_playerPrefab, _spawnPoint.transform.position, _spawnPoint.transform.rotation);
        }
    }
}
