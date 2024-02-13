using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SimpleScaler : MonoBehaviour
{
    public Vector3 scaleAmount = new Vector3(1.1f , 1.1f , 1.1f);
    public float scaleTime = 0.5f;
    public int vibration = 1;
    public float elasticity = 1;
    
    public bool loop;
    public bool playOnStart;

    private void Start()
    {
        if (playOnStart)
            Animate();
    }

    private void Animate()
    {
        Tween tween = transform.DOPunchScale(scaleAmount, scaleTime , vibration , elasticity);
        
        if (loop)
        {
            tween.SetLoops(-1, LoopType.Restart);
        }
    }
}
