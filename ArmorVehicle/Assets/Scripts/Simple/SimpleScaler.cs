using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SimpleScaler : MonoBehaviour
{
    public Vector3 scaleAmount = new Vector3(0.1f , 0.1f , 0.1f);
    public float scaleTime = 0.5f;
    public int vibration = 1;
    public float elasticity = 1;
    
    public bool loop;
    public bool playOnStart;

    private Vector3 _startLocalScale;

    private void Awake()
    {
        _startLocalScale = transform.localScale;
        
        if (playOnStart)
            Animate();
    }

    private void Animate()
    {
        transform.localScale = _startLocalScale;
        
        Tween tween = transform.DOPunchScale(scaleAmount, scaleTime , vibration , elasticity);
        
        if (loop)
        {
            tween.SetLoops(-1, LoopType.Restart);
        }
    }

    public void PlayOnce()
    {
        Animate();
    }
}
