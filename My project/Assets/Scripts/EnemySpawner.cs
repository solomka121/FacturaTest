using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy _prefab;
    private List<Enemy> _enemies;

    private void Awake()
    {
        _enemies = new List<Enemy>();
    }

    public void SpawnEnemies(Level level)
    {
        float spawnLength = level.SpawnZoneLenght;
        float distanceBetweenEnemies = spawnLength / level.enemiesToSpawn;

        LevelData levelData = level.levelData;

        Vector3 spawnPosition = Vector3.zero;
        spawnPosition.z = level.startPoint.position.z;

        for (int i = 0; i < level.enemiesToSpawn; i++)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(-levelData.xRandomness, levelData.xRandomness),
                0,
                Random.Range(-levelData.zRandomness, levelData.zRandomness));
            
            Enemy enemy = Instantiate(_prefab, spawnPosition + randomPos, _prefab.transform.rotation, transform);

            _enemies.Add(enemy);
            spawnPosition.z += distanceBetweenEnemies;
        }
    }
}