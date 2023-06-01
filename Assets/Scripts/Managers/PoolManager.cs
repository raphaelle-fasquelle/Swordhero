using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private readonly List<Pool<Component>> _pools = new List<Pool<Component>>();

    /// <summary>
    /// Generic get pool method creating and returning a pool from a given prefab.
    /// </summary>
    public Pool<Component> GetPool<T>(T poolPrefab) where T : Component
    {
        Pool<Component> result = _pools.Find(pool => pool.Prefab.Equals(poolPrefab));
        if (result == null)
        {
            result = new Pool<Component>(poolPrefab, this);
            _pools.Add(result);
        }
        return result;
    }

}
