using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy _prefab;
    [SerializeField] private int _preload = 20;
    [SerializeField] private float _distanceToSpawn = 10;
    [SerializeField] private Player _player;
    [SerializeField] private ParticleItemsPool damageParticleItemsPool;
    [SerializeField] private Queue<Enemy> _enemiesPool;
    private List<Enemy> _activeEnemies;
    
    [SerializeField] private List<SpawnPoint> _spawnPoints;
    [SerializeField] private SpawnPoint _spawnPointPrefab;
    [SerializeField] private Transform _spawnPointsParent;
    private Vector3 _lastSpawnedPoint;
    
    private Level _currentLevel;

    private void Awake()
    {
        _enemiesPool = new Queue<Enemy>();
        _activeEnemies = new List<Enemy>();
        
        _spawnPoints = new List<SpawnPoint>();

        for (int i = 0; i < _preload; i++)
        {
            _enemiesPool.Enqueue(SpawnEnemy());
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < _spawnPoints.Count; i++)
        {
            if(_spawnPoints[i].gameObject.activeSelf == false)
                continue;
            
            if(_spawnPoints[i].spawned)
                continue;
            
            if (Vector3.Distance(_spawnPoints[i].transform.position, _player.transform.position) > _distanceToSpawn)
                return;
            
            SetEnemy(_spawnPoints[i].transform.position);
            _spawnPoints[i].spawned = true;
        }
    }

    public void SpawnEnemyPoints(Level level)
    {
        ClearSpawnPoints();
        
        _currentLevel = level;
        
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

            SetSpawnPoint(spawnPosition + randomPos , i);

            spawnPosition.z += distanceBetweenEnemies;
        }
        
        DeactivateExtraSpawnPoints(level.enemiesToSpawn);
    }

    private void SetSpawnPoint(Vector3 position , int index)
    {
        if(index >= _spawnPoints.Count)
            _spawnPoints.Add(Instantiate(_spawnPointPrefab , _spawnPointsParent));
        
        _spawnPoints[index].gameObject.SetActive(true);
        _spawnPoints[index].transform.position = position;
    }

    private void DeactivateExtraSpawnPoints(int count)
    {
        for (int i = count; i < _spawnPoints.Count; i++)
        {
            _spawnPoints[i].gameObject.SetActive(false);
        }
    }

    private Enemy GetEnemy()
    {
        if (_enemiesPool.Count == 0)
        {
            return SpawnEnemy();
        }
        
        return _enemiesPool.Dequeue();
    }

    public void SetEnemy(Vector3 spawnPosition)
    {
        Enemy enemy = GetEnemy();
        enemy.transform.position = spawnPosition;
        enemy.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(-180, 180), 0));
        enemy.SetLevelData(_currentLevel);
        
        enemy.gameObject.SetActive(true);
        _activeEnemies.Add(enemy);
    }

    private Enemy SpawnEnemy()
    {
        Enemy enemy = Instantiate(_prefab, transform);
        enemy.gameObject.SetActive(false);
        enemy.Init(this, _player, damageParticleItemsPool);
        
        return enemy;
    }

    public void ReturnToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        enemy.Reset();
        
        _enemiesPool.Enqueue(enemy);
        _activeEnemies.Remove(enemy);
    }
    
    public void ClearEnemies()
    {
        for (int i = _activeEnemies.Count - 1; i >= 0; i--)
        {
            _activeEnemies[i].ReturnToPool();
        }
    }

    public void ClearAggro()
    {
        for (int i = _activeEnemies.Count - 1; i >= 0; i--)
        {
            _activeEnemies[i].ClearAggro();
        }
    }
    
    private void ClearSpawnPoints()
    {
        for (int i = 0; i < _spawnPoints.Count; i++)
        {
            _spawnPoints[i].gameObject.SetActive(false);
            _spawnPoints[i].spawned = false;
        }
    }
}