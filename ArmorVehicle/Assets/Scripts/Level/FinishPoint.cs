using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    public event System.Action OnFinishTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out _))
        {
            OnFinishTrigger?.Invoke();
            Debug.Log("yo");
        }
    }
}
