using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelsController : MonoBehaviour
{
    [Header("UI")] 
    [SerializeField] private GameObject _tapToPlayPanel;
    [SerializeField] private LosePanel _losePanel;
    [SerializeField] private WinPanel _winPanel;
    
    [Header("Scene")]
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private Player _player;
    [SerializeField] private CinemachineVirtualCamera _rideCamera;
    
    [Header("Levels")]
    [SerializeField] private List<Level> _levels;
    private Level currentLevel => _levels[_currentLevelIndex];
    private int _currentLevelIndex;

    private bool _isPlayingLevel;

    private void Start()
    {
        _player.health.OnDeath += LevelFail;

        LoadLevel();
    }

    private void ResetLevel()
    {
        _player.Reset();
        currentLevel.finishPoint.OnFinishTrigger -= LevelWin;
        _tapToPlayPanel.gameObject.SetActive(false);

        _rideCamera.Priority = -1;

        _isPlayingLevel = false;
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

    public void LoadNextLevel()
    {
        _winPanel.Hide();
        
        ClearLevel();
        ResetLevel();
        _currentLevelIndex = (_currentLevelIndex + 1) % _levels.Count;
        LoadLevel();
    }

    public void LoadLevel()
    {
        _tapToPlayPanel.gameObject.SetActive(true);
        currentLevel.gameObject.SetActive(true);
        _enemySpawner.SpawnEnemyPoints(currentLevel);
        
        currentLevel.finishPoint.OnFinishTrigger += LevelWin;
    }
    
    public void StartLevel()
    {
        _tapToPlayPanel.gameObject.SetActive(false);
         
        _player.Activate();
        
        _isPlayingLevel = true;

        _rideCamera.Priority = 2;
    }

    public void ClearLevel()
    {
        currentLevel.gameObject.SetActive(false);
        _enemySpawner.ClearEnemies();
        
        currentLevel.finishPoint.OnFinishTrigger -= LevelWin;
    }

    public void LevelFail()
    {
        _losePanel.Show();
    }

    public void LevelWin()
    {
        _winPanel.Show();
        
        _player.Deactivate();
        _enemySpawner.ClearAggro();
    }

    public void RestartLevel()
    {
        _losePanel.Hide();
        
        ClearLevel();
        ResetLevel();
        LoadLevel();
    }
}
