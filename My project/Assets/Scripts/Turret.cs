using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _maxRotationAngle = 60;
    [SerializeField] private float _rotationSpeed = 20;
    
    [SerializeField] private Transform _barrelPoint;
    [SerializeField] private Transform _laser;

    private float _targetYAngle = 0;

    private void Update()
    {
        RotateToTargetAngle();
    }

    private void RotateToTargetAngle()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, _targetYAngle, 0)),
            _rotationSpeed * Time.deltaTime);
    }

    public void ChangeRotationTarget(float rotateAmount)
    {
        _targetYAngle += rotateAmount;

        _targetYAngle = (_targetYAngle > 180) ? _targetYAngle - 360 : _targetYAngle;
        _targetYAngle = Mathf.Clamp(_targetYAngle, -_maxRotationAngle, _maxRotationAngle);
    }
}