using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO generic pool
public class ParticlesPool : MonoBehaviour
{
    [SerializeField] private ParticlePoolItem _prefab;
    [SerializeField] private Queue<ParticlePoolItem> _items;
    [SerializeField] private int _preloadedItems = 10;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        for (int i = 0; i < _preloadedItems; i++)
        {
            Add(SpawnItem());
        }
    }

    public void ActivateParticleAt(Vector3 position , Quaternion rotation)
    {
        ParticlePoolItem particle = GetItem();
        particle.transform.position = position;
        particle.transform.rotation = rotation;
        particle.Activate();
    }

    public ParticlePoolItem GetItem()
    {
        if(_items.Count == 0)
            Add(SpawnItem());

        ParticlePoolItem item = _items.Dequeue();

        return item;
    }

    public void ReturnItem(ParticlePoolItem item)
    {
        Add(item);
    }

    private void Add(ParticlePoolItem item)
    {
        item.gameObject.SetActive(false);
        _items.Enqueue(item);
    }

    private ParticlePoolItem SpawnItem()
    {
        ParticlePoolItem item = Instantiate(_prefab, transform);
        item.Init(this);
        
        return item;
    }
}
