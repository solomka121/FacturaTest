using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Health health;
    private Tween _damageTween;

    [SerializeField] private float _maxSpeed = 2;
    [SerializeField] private float _accelerationSpeed = 2;
    private float _currentSpeed;
    private Rigidbody _rigidbody;

    [SerializeField] private Transform _visual;
    [SerializeField] private Transform _carVisual;
    [SerializeField] private Turret _turret;
    [SerializeField] private float _rotationSensitivity = 100;

    [SerializeField] private LayerMask _enemies;
    [SerializeField] private float _noiseRadius = 8;

    private Vector3 _startPoint;
    public bool canMove = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        health.OnHealthChange += Damage;
        health.OnDeath += Death;
    }

    public void Activate()
    {
        canMove = true;
        
        StartCoroutine(SpeedUp());
        _turret.Activate();
    }

    public void Death()
    {
        canMove = false;
        _turret.Deactivate();
    }

    private IEnumerator SpeedUp()
    {
        float progress = 0;
        while (progress <= 1)
        {
            _currentSpeed = Mathf.Lerp(0, _maxSpeed, progress);
            progress += Time.deltaTime * _accelerationSpeed;

            yield return null;
        }

        _currentSpeed = _maxSpeed;
    }

    private void Update()
    {
        CheckForInput();
    }

    private void FixedUpdate()
    {
        if (!canMove)
            return;

        MoveForward();
        MakeNoise();
    }

    private void Damage(float _)
    {
        DOTween.Kill(_carVisual);
        _carVisual.localPosition = Vector3.zero;
        _carVisual.localScale = Vector3.one;

        _carVisual.DOShakePosition(0.2f, 0.14f);
        _carVisual.DOPunchScale(Vector3.one * -0.12f, 0.16f);
    }

    private void CheckForInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _startPoint = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                float moveValue = touch.deltaPosition.x / Screen.width * _rotationSensitivity;

                _turret.ChangeRotationTarget(moveValue);

                _startPoint = Input.mousePosition;
            }

            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _startPoint = Input.mousePosition;
            return;
        }

        if (Input.GetMouseButton(0))
        {
            float moveValue = (Input.mousePosition.x - _startPoint.x) / Screen.width * _rotationSensitivity;

            _turret.ChangeRotationTarget(moveValue);

            _startPoint = Input.mousePosition;
        }
    }

    private void MoveForward()
    {
        transform.position += Vector3.forward * (_currentSpeed * Time.deltaTime);
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
        Gizmos.DrawWireSphere(transform.position, _noiseRadius);
    }
}