using System.Collections;
using System.Linq.Expressions;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Health health;
    public HealthBar healthBar;
    private Tween _damageTween;

    [SerializeField] private float _maxSpeed = 2;
    [SerializeField] private float _accelerationSpeed = 2;
    [SerializeField] private float _sineStrength = 2;
    private float _currentSineStrength = 0;
    [SerializeField] private float _sineAmplitude = 1;
    private float _startMovingTime;
    [SerializeField] private float _startMovingOffset = 1.57f; // pi / 2.
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

    public void Reset()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        health.Reset();
        healthBar.Hide();
        healthBar.UpdateValue();
        _turret.Reset();
        Deactivate();
    }

    public void Activate()
    {
        healthBar.Show();
        canMove = true;

        _startMovingTime = Time.time;
        _currentSineStrength = 0;

        StartCoroutine(SpeedUp());
        _turret.Activate();
    }

    public void Deactivate()
    {
        canMove = false;
        _turret.Deactivate();
    }

    public void Death()
    {
        Deactivate();
    }

    private IEnumerator SpeedUp()
    {
        float progress = 0;
        while (progress <= 1)
        {
            _currentSpeed = Mathf.LerpUnclamped(0, _maxSpeed, progress);
            progress += Time.deltaTime * _accelerationSpeed;

            yield return new WaitForFixedUpdate();
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
        Vector3 previousPosition = transform.position;
        transform.position += Vector3.forward * (_currentSpeed * Time.deltaTime);
        float timePassedSinceMove = Time.time - _startMovingTime - 2f;

        if (timePassedSinceMove < 0)
            return;
        
        TransformInSineWave(previousPosition, timePassedSinceMove);
        _currentSineStrength = Mathf.Clamp01(_currentSineStrength + (Time.deltaTime * 0.25f));
    }

    private void TransformInSineWave(Vector3 previousPosition , float timePassedSinceMove)
    {
        float xSinePosition = Mathf.Sin((timePassedSinceMove) * _sineAmplitude);
        transform.position = new Vector3(xSinePosition * _sineStrength * _currentSineStrength , transform.position.y, transform.position.z);
        
        Vector3 lookDirection = transform.position - previousPosition;
        transform.rotation =
            Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), 6 * Time.deltaTime);

        Debug.DrawLine(transform.position, previousPosition,
            new Color(
                (Time.time * 12) % 1, 0, 0, 1), 8);
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