using UnityEngine;

namespace  Projects.NetworkUtility.ObjectPool.Scripts
{
    public class PoolTestManager : MonoBehaviour
    {
        [SerializeField] Transform parent1;
        [SerializeField] Transform parent2;
        GameObjectPool pool;

        void Start()
        {
            pool = new GameObjectPool(parent1, new GameObject("TestObject"), 10);
            pool = new GameObjectPool(parent2, new GameObject("Tank"), 3);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var child = pool.Get();
                Debug.Log(child.name);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                var find = GameObject.Find("TestObject(Clone)");
                pool.Release(find.gameObject);
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                var find = GameObject.Find("Tank");
                pool.Release(find.gameObject);
            }

            if (Input.GetKeyDown(KeyCode.C)) pool.Clear();
        }
    }
}