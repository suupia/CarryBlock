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
            var find = GameObject.Find("Picker");
            pool.Release(find.gameObject);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            var find = GameObject.Find("Tank");
            pool.Release(find.gameObject);
        }
        
        if(Input.GetKeyDown(KeyCode.C))
        {
            pool.Clear();
        }

    }
}

