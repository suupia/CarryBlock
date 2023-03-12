using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform resourcesParent;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform enemiesParent;
    [SerializeField] int spawnNumber;
    float spawnRadius = 100;
    Collider shootRangeCollider;

    void Start()
    {

        for(int i = 0;i< spawnNumber; i++)
        {
            var x = Random.Range(-spawnRadius, spawnRadius);
            var z = Random.Range(-spawnRadius, spawnRadius);
            var enemy= Instantiate(enemyPrefab, new Vector3(x, 0.5f, z),Quaternion.identity,enemiesParent).GetComponent<EnemyController>();
            enemy.Init(resourcesParent);
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }
}
