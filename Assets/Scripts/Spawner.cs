using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnPointsNest;
    [SerializeField] private GameObject enemiesNest;
    [SerializeField] private GameObject[] prefabs; // TODO
    private Transform[] _spawnPoints;

    private void Awake()
    {
        _spawnPoints = spawnPointsNest.GetComponentsInChildren<Transform>();
    }

    private void OnEnable() => Movement.OnEnemyEaten += SpawnEnemy;

    private void SpawnEnemy()
    {
        var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        Instantiate(prefabs[Random.Range(0, prefabs.Length)], spawnPoint.position, Quaternion.identity, enemiesNest.transform);
        Debug.Log("Enemy spawned");
    }
}
