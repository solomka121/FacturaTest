using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public Health health;
    [SerializeField] private Vector3 _centerOffset = new Vector3(0 , 0.5f , 0);
    private ParticleItemsPool _damageParticleItemsPool;

    [SerializeField] private float _runSpeed = 2;
    private float _moveMultiplier;
    [SerializeField] private float _rotationSpeed = 4;
    [SerializeField] private float _damage = 5;
    [SerializeField] private Animator _animator;

    [Header("Walk")] 
    [SerializeField] private bool _walk = true;
    [SerializeField] private float _walkSpeed = 1;
    [SerializeField] private float _walkFrequency = 5f;
    [SerializeField] private float _walkFrequencyRandomness = 4f;
    [SerializeField] private float _maxWalkDistance = 1f;
    [SerializeField] private float _minWalkDistance = 0.2f;
    private float _maxXWalkValidPoint;
    
    private float _timeRemainingToWalk;
    private Coroutine _walking;
    private bool _isWalking;
    
    private bool _isAngry;
    private Player _player;

    public void Init(Player player , ParticleItemsPool damageParticleItemsPool , float maxXValidPoint)
    {
        _player = player;
        _damageParticleItemsPool = damageParticleItemsPool;
        _maxXWalkValidPoint = maxXValidPoint;

        health.OnDeath += Death;
        
        UpdateWalkTimer();
        SetAnimatorRandomIdleOffset();
    }

    private void FixedUpdate()
    {
        if (!_isAngry)
        {
            TryWalk();
            return;
        }
        
        RotateToTarget(_player.transform.position , true);
        MoveToTarget(_player.transform.position , true);
        SetRunningAnimation();
    }

    private void TryWalk()
    {
        if (_isWalking)
            return;
            
        _timeRemainingToWalk -= Time.deltaTime;
        CheckIfNeedToWalk();
    }
    
    private void CheckIfNeedToWalk()
    {
        if (_timeRemainingToWalk <= 0)
        {

            _walking = StartCoroutine(WalkCoroutine());
            _isWalking = true;

            UpdateWalkTimer();
        }
    }

    private void UpdateWalkTimer()
    {
        _timeRemainingToWalk = _walkFrequency + Random.Range(-_walkFrequencyRandomness, _walkFrequencyRandomness);
    }

    private IEnumerator WalkCoroutine()
    {
        _isWalking = true;

        Vector3 walkPoint = transform.position;
        walkPoint += new Vector3(
        Mathf.Clamp(Random.insideUnitCircle.x * _maxWalkDistance , _minWalkDistance , _maxWalkDistance),
        0 ,
        Mathf.Clamp(Random.insideUnitCircle.y * _maxWalkDistance , _minWalkDistance , _maxWalkDistance));
        walkPoint.x = Mathf.Clamp(walkPoint.x, -_maxXWalkValidPoint, _maxXWalkValidPoint);

        float distanceToWalkPoint = (walkPoint - transform.position).magnitude;
        
        while (distanceToWalkPoint > 0.1f)
        {
            RotateToTarget(walkPoint);
            MoveToTarget(walkPoint);
            SetAnimatorSpeed();
            
            distanceToWalkPoint = (walkPoint - transform.position).magnitude;

            yield return null;
        }

        _isWalking = false;
        ResetAnimatorSpeed();
    }

    private void RotateToTarget(Vector3 target , bool run = false)
    {
        _moveMultiplier = Mathf.Clamp(Vector3.Dot(transform.position, target), 0, 1);

        Vector3 directionToPlayer = target - transform.position;
        
        float targetAngle = Mathf.Atan2(directionToPlayer.x , directionToPlayer.z ) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.AngleAxis(targetAngle, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private void MoveToTarget(Vector3 target , bool run = false)
    {
        Vector3 directionToPlayer = target - transform.position;

        transform.position += transform.forward * ((run ? _runSpeed : _walkSpeed) * _moveMultiplier * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.health.Damage(_damage);
            health.Death();
        }
    }

    public void Damage(Vector3 damagePoint , float damage)
    {
        health.Damage(damage);
        SetAnimatorDamage();
        
        damagePoint.y = transform.position.y + _centerOffset.y;
        _damageParticleItemsPool.ActivateParticleAt(damagePoint , Quaternion.LookRotation(damagePoint - transform.position));
    }

    private void Death()
    {
        _damageParticleItemsPool.ActivateParticleAt(transform.position + _centerOffset , Quaternion.LookRotation(-transform.forward));
        Destroy(gameObject);
    }

    private void SetAnimatorRandomIdleOffset()
    {
        _animator.SetFloat("IdleOffset" , Random.Range(0 , 1));
    }

    private void SetRunningAnimation()
    {
        _animator.SetBool("Running" , _moveMultiplier > 0.1f ? true : false);
        SetAnimatorSpeed();
    }

    private void SetAnimatorSpeed()
    {
        _animator.SetFloat("Speed" , _moveMultiplier);
    }
    
    private void SetAnimatorDamage()
    {
        _animator.SetTrigger("Damage");
    }

    private void ResetAnimatorSpeed()
    {
        _animator.SetFloat("Speed" , 0);
    }

    public void Aggro()
    {
        _isAngry = true;
        
        if (_walking != null)
            StopCoroutine(_walking);
    }
}
