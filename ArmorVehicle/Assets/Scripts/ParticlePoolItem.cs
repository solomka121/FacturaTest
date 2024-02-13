using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticlePoolItem : MonoBehaviour
{
    public ParticleSystem particles;
    private ParticlesPool _pool;
    private float _duration;

    private void OnValidate()
    {
        if (particles == null)
            particles = GetComponent<ParticleSystem>();
    }

    public void Init(ParticlesPool pool)
    {
        _pool = pool;
        _duration = particles.main.duration;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        particles.Play();

        StartCoroutine(ResetWithDelay(_duration));
    }

    private IEnumerator ResetWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Reset();
    }
    
    public void Reset()
    {
        particles.Clear();
        _pool.ReturnItem(this);
    }
}
