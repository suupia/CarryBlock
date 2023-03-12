using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int spawnNumber;
    float spawnRadius = 100;
    Collider shootRangeCollider;

    void Start()
    {


        for(int i = 0;i< spawnNumber; i++)
        {
            var x = Random.Range(-spawnRadius, spawnRadius);
            var z = Random.Range(-spawnRadius, spawnRadius);
            var enemy= Instantiate(enemyPrefab, new Vector3(x, 0.5f, z),Quaternion.identity).GetComponent<EnemyController>();
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }
}
