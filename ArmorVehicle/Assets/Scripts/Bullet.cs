using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private BulletItemsPool _pool;
    private Rigidbody _rigidbody;

    private float _damage;
    private float _speed;

    [SerializeField] private float _lifetime = 3;
    private float _timePassed;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * (_speed * Time.deltaTime);

        _timePassed += Time.deltaTime;
        if (_timePassed > _lifetime)
        {
            Reset();
        }
    }

    public void SetPool(BulletItemsPool pool)
    {
        _pool = pool;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.Damage(transform.position , _damage);
            Reset();
        }
    }

    public void Init(float damage , float speed)
    {
        _damage = damage;
        _speed = speed;
        
        gameObject.SetActive(true);
    }

    private void Reset()
    {
        _timePassed = 0;
        _pool.ReturnItem(this);
    }
}
