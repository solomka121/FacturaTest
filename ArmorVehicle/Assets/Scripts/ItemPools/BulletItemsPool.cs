using System.Collections.Generic;
using UnityEngine;

public class BulletItemsPool : MonoBehaviour
{
    [SerializeField] private Bullet _prefab;
    [SerializeField] private Queue<Bullet> _items;
    [SerializeField] private int _preloadedItems = 10;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _items = new Queue<Bullet>();
        
        for (int i = 0; i < _preloadedItems; i++)
        {
            Add(SpawnItem());
        }
    }

    public Bullet GetItem()
    {
        if(_items.Count == 0)
            Add(SpawnItem());

        Bullet item = _items.Dequeue();

        return item;
    }

    public void ReturnItem(Bullet item)
    {
        Add(item);
    }

    private void Add(Bullet item)
    {
        item.gameObject.SetActive(false);
        _items.Enqueue(item);
    }

    private Bullet SpawnItem()
    {
        Bullet item = Instantiate(_prefab);
        item.SetPool(this);
        
        return item;
    }
}
