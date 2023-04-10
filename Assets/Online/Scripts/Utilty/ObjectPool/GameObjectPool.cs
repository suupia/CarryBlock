using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Object = System.Object;

# nullable enable
public class GameObjectPool
{
    readonly GameObject[] _pool;
    readonly List<GameObject> _activeObjects = new List<GameObject>();
    readonly int _poolSize;

    public int CountActive => _pool.Count(obj => obj.activeSelf);

    public int CountInactive => _pool.Count(obj => !obj.activeSelf);

    public GameObjectPool(Transform parent,GameObject prefab, int poolSize = 20)
    {
        if (poolSize <= 0)
        {
            throw new ArgumentException("Parent does not have any of these objects.", nameof(poolSize));
        }
        _pool = new GameObject[poolSize];
        for (int i = 0; i< poolSize; i++)
        {
            var obj = UnityEngine.Object.Instantiate(prefab, parent);
            obj.SetActive(false);
            _pool[i] = obj;
        }
    }

    public GameObject Get()
    {
        var obj = _pool.FirstOrDefault(obj => !obj.activeSelf);
        if (obj == null)
        {
            obj = _activeObjects.First();
            SetActiveFalseFirst(obj);
            SetActiveTrue(obj);
            Debug.LogWarning($"There were not enough pooled objects, so I reactivated the oldest object that became active. poolSize = {_pool.Length}");
        }
        else
        {
            SetActiveTrue(obj);
        }
        return obj;
    }

    public void Release(GameObject obj)
    {
        // Check if the object has been registered with this pool
        if (_pool.All(e => e != obj))
        {
            throw new ArgumentException("Attempting to release an object that was not registered with this pool",
                nameof(obj));
        }

        // Check if the object is already inactive
        if (obj.activeSelf == false)
            throw new ArgumentException("Attempting to release an object that is already inactive", nameof(obj));

        SetActiveFalseLast(obj);
    }

    public void Clear()
    {
        foreach (var obj in _pool)
        {
            obj.SetActive(false);
        }
        _activeObjects.Clear();
    }

    void SetActiveTrue(GameObject obj)
    {
        obj.SetActive(true);
        _activeObjects.Add(obj);
    }
    void SetActiveFalseFirst(GameObject obj)
    {
        obj.SetActive(false);
        _activeObjects.RemoveAt(0);
    }
    
    void SetActiveFalseLast(GameObject obj)
    {
        obj.SetActive(false);
        _activeObjects.RemoveAt(_activeObjects.Count()-1);
    }
}