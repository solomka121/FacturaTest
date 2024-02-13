using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 10;
    private float _currentSpeed;
    private Rigidbody _rigidbody;
    
    [SerializeField] private Turret _turret;
    [SerializeField] private float _rotationSensitivity = 10;

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
        RideForward();
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

    private void RideForward()
    {
        transform.position += Vector3.forward * (_speed * Time.deltaTime);
    }
}
