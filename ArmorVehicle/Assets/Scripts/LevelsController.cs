using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsController : MonoBehaviour
{
    [SerializeField] private List<Level> _levels;
    [SerializeField] private EnemySpawner _enemySpawner;

    private Level currentLevel => _levels[_currentLevelIndex];
    private int _currentLevelIndex;

    private void Start()
    {
        LoadLevel();
    }

    public void StartLevel()
    {
        
    }

    public void LoadLevel()
    {
        _enemySpawner.SpawnEnemies(currentLevel);
    }
}
