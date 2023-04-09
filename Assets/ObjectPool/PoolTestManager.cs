using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using System;
using System.Data;
using Object = System.Object;

public class PoolTestManager : MonoBehaviour
{
    GameObjectPool pool;
    [SerializeField] Transform parent;

    void Start()
    {
        pool = new GameObjectPool(parent, 20, "Picker");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           var child =   pool.Get();
           Debug.Log(child.name);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            var find = GameObject.Find("Apple");
            pool.Release(find.gameObject);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            var find = GameObject.Find("Banana");
            pool.Release(find.gameObject);
        }
        
        if(Input.GetKeyDown(KeyCode.C))
        {
            var find = GameObject.Find("Cherry");
            pool.Release(find.gameObject);
        }
        
        if(Input.GetKeyDown(KeyCode.D))
        {
            pool.Clear();;
        }
    }
}


public class GameObjectPool
{
    readonly List<GameObject> _pool;
    readonly int _poolSize;

    public int CountActive => _pool.Count(obj => obj.activeSelf);

    public int CountInactive => _pool.Count(obj => !obj.activeSelf);

    public GameObjectPool(Transform parent, int poolSize = 20, string name = "Pool")
    {
        if (poolSize <= 0)
            throw new ArgumentException("Parent does not have any of these objects.", nameof(poolSize));
        _pool = new List<GameObject>();
        for (int i = 0; i< poolSize; i++)
        {
            var obj = new GameObject(name);
            obj.transform.SetParent(parent);
            obj.SetActive(false);
            _pool.Add(obj);
        }
    }

    public GameObject Get()
    {
        var obj = _pool.FirstOrDefault(obj => !obj.activeSelf);
        if (obj == null)
            throw new ArgumentException("Inactive pooled objects are missing.", nameof(_poolSize));
        obj.SetActive(true);
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

        obj.SetActive(false);
    }

    public void Clear()
    {
        foreach (var obj in _pool)
        {
            obj.SetActive(false);
        }
    }
}