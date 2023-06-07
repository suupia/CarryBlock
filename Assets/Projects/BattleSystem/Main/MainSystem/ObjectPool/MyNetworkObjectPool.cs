using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Main
{
# nullable enable
    public class MyNetworkObjectPool
    {
        readonly List<GameObject> _activeObjects = new();
        readonly GameObject[] _pool;
        readonly int _poolSize;

        public MyNetworkObjectPool(Transform parent)
        {
            var poolSize = parent.childCount;
            if (poolSize <= 0) throw new ArgumentException("Parent does not have any of children.", nameof(poolSize));
            _pool = new GameObject[poolSize];

            for (var i = 0; i < poolSize; i++)
            {
                var obj = parent.GetChild(i).gameObject;
                obj.SetActive(false);
                _pool[i] = obj;
            }
        }

        public int CountActive => _pool.Count(obj => obj.activeSelf);

        public int CountInactive => _pool.Count(obj => !obj.activeSelf);

        public GameObject Get()
        {
            var obj = _pool.FirstOrDefault(obj => !obj.activeSelf);
            if (obj == null)
            {
                obj = _activeObjects.First();
                OnRelease(obj, true);
                SetActiveTrue(obj);
                Debug.LogWarning(
                    $"There were not enough pooled objects, so I reactivated the oldest object that became active. poolSize = {_pool.Length}");
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
                throw new ArgumentException("Attempting to release an object that was not registered with this pool",
                    nameof(obj));

            // Check if the object is already inactive
            if (obj.activeSelf == false)
                throw new ArgumentException("Attempting to release an object that is already inactive", nameof(obj));

            OnRelease(obj, false);
        }

        public void Clear()
        {
            foreach (var obj in _pool) OnRelease(obj, true);
            _activeObjects.Clear();
        }

        void SetActiveTrue(GameObject obj)
        {
            obj.SetActive(true);
            _activeObjects.Add(obj);
        }

        void OnRelease(GameObject obj, bool releaseFirst)
        {
            obj.SetActive(false);
            obj.transform.localPosition = Vector3.zero;
            var rds = obj.GetComponentsInChildren<Rigidbody>();
            if (rds.Any())
                foreach (var rd in rds)
                {
                    rd.velocity = Vector3.zero;
                    rd.angularVelocity = Vector3.zero;
                }

            if (releaseFirst)
                _activeObjects.RemoveAt(0);
            else
                _activeObjects.RemoveAt(_activeObjects.Count() - 1);
        }
    }
}