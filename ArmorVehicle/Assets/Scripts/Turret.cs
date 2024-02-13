using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private float _damage = 5;
    [SerializeField] private float _bulletSpeed = 5;
    [SerializeField] private float _fireRate = 1;
    private float _timeSinceLastShot;
    [SerializeField] private BulletItemsPool _bulletsPool;
    
    [SerializeField] private float _maxRotationAngle = 60;
    [SerializeField] private float _rotationSpeed = 20;
    
    [SerializeField] private Transform _barrelPoint;
    [SerializeField] private Transform _laser;

    private float _targetYAngle = 0;

    private void FixedUpdate()
    {
        RotateToTargetAngle();
        TryShoot();
    }

    private void TryShoot()
    {
        if (_timeSinceLastShot > 1 / _fireRate)
        {
            Shoot();
            _timeSinceLastShot = 0;
        }

        _timeSinceLastShot += Time.deltaTime;
    }

    private void Shoot()
    {
        Bullet bulet =  _bulletsPool.GetItem();
        
        bulet.transform.rotation = _barrelPoint.rotation;
        bulet.transform.position = _barrelPoint.position;
        bulet.Init(_damage , _bulletSpeed);
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