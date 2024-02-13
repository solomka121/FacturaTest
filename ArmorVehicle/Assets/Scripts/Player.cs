using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 10;
    private float _currentSpeed;
    private Rigidbody _rigidbody;
    
    [SerializeField] private Turret _turret;
    [SerializeField] private float _rotationSensitivity = 10;

    [SerializeField] private LayerMask _enemies;
    [SerializeField] private float _noiseRadius = 8;

    private Vector3 startPoint;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckForInput();
    }

    private void FixedUpdate()
    {
        MoveForward();
        MakeNoise();
    }

    private void CheckForInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startPoint = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                float moveValue = touch.deltaPosition.x / Screen.width * _rotationSensitivity;
            
                _turret.ChangeRotationTarget(moveValue);
        
                startPoint = Input.mousePosition;
            }
        }
    }

    private void MoveForward()
    {
        transform.position += Vector3.forward * (_speed * Time.deltaTime);
    }

    private void MakeNoise()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _noiseRadius);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.Aggro();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position , _noiseRadius);
    }
}
