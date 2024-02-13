using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool _invert = false;
    [SerializeField] private bool _destroyOnStart = false;
    [SerializeField] private bool _ignoreY = false;
    [SerializeField] private bool _ignoreX = false;
    private Camera _targetCamera;

    private void Start()
    {
        _targetCamera = Camera.main;
        RotateToCamera();
        
        if(_destroyOnStart)
            Destroy(this);
    }

    private void LateUpdate()
    {
        RotateToCamera();
    }

    private void RotateToCamera()
    {
        Vector3 lookAtTarget = _targetCamera.transform.position;
        if (_ignoreX)
            lookAtTarget.x = transform.position.x;
        if (_ignoreY)
            lookAtTarget.y = transform.position.y;
        
        Vector3 direction = _invert
            ? transform.position - lookAtTarget
            : lookAtTarget - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
