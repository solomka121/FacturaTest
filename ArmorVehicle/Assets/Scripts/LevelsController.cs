using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelsController : MonoBehaviour
{
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private Player _player;
    [SerializeField] private CinemachineVirtualCamera _rideCamera;
    
    [SerializeField] private List<Level> _levels;
    private Level currentLevel => _levels[_currentLevelIndex];
    private int _currentLevelIndex;

    private bool _isPlayingLevel;

    private void Start()
    {
        SetLevel();
    }

    private void Update()
    {
        if (_isPlayingLevel)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            StartLevel();
        }
    }

    private void SetLevel()
    {
        _currentLevelIndex = 0;
        LoadLevel();
    }

    public void StartLevel()
    {
        _player.Activate();
        _enemySpawner.ActivateEnemies();
        
        _isPlayingLevel = true;

        _rideCamera.Priority = 2;
    }

    public void LoadLevel()
    {
        currentLevel.gameObject.SetActive(true);
        _enemySpawner.SpawnEnemies(currentLevel);
    }
}
