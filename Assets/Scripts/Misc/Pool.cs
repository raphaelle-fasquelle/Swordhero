using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : Component
{
    public T Prefab => _prefab;

    private Queue<T> _queue;
    private GameObject _usedObjects;
    private GameObject _availableObjects;
    private T _prefab;
    
    public Pool(T prefab, MonoBehaviour parent)
    {
        _prefab = prefab;
        GameObject pool = new GameObject
        {
            transform =
            {
                parent = parent.transform
            },
            name = "Pool - "+prefab.name
        };
        _usedObjects = new GameObject
        {
            transform =
            {
                parent = pool.transform
            },
            name = "Objects in use"
        };
        _availableObjects = new GameObject
        {
            transform =
            {
                parent = pool.transform
            },
            name = "Objects available"
        };
        _queue = new Queue<T>();
    }

    public T Pick()
    {
        if (_queue.Count > 0)
        {
            T pickedInstance = _queue.Dequeue();
            pickedInstance.transform.SetParent(_usedObjects.transform);
            pickedInstance.gameObject.SetActive(true);
            return pickedInstance;
        }
        return GameObject.Instantiate(_prefab, _usedObjects.transform);
    }

    public void ReturnToPool(T objectToReturn)
    {
        _queue.Enqueue(objectToReturn);
        objectToReturn.gameObject.SetActive(false);
        objectToReturn.transform.SetParent(_availableObjects.transform); 
    }
}
