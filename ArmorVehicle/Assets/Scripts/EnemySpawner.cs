using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy _prefab;
    [SerializeField] private Player _player;
    private List<Enemy> _enemies;

    private void Awake()
    {
        _enemies = new List<Enemy>();
    }

    public void SpawnEnemies(Level level)
    {
        float spawnLength = level.SpawnZoneLenght;
        float distanceBetweenEnemies = spawnLength / level.enemiesToSpawn;

        LevelSpawnData levelSpawnData = level.levelSpawnData;

        Vector3 spawnPosition = Vector3.zero;
        spawnPosition.z = level.startPoint.position.z;

        for (int i = 0; i < level.enemiesToSpawn; i++)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(-levelSpawnData.xRandomness, levelSpawnData.xRandomness),
                0,
                Random.Range(-levelSpawnData.zRandomness, levelSpawnData.zRandomness));

            Enemy enemy = Instantiate(_prefab, spawnPosition + randomPos,
                Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)),
                transform);
            enemy.Init(_player , levelSpawnData.maxXValidPoint);

            _enemies.Add(enemy);
            spawnPosition.z += distanceBetweenEnemies;
        }
    }
}