using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] int spawnNumber;
    float spawnRadius = 100;

    void Start()
    {
        for (int i = 0; i < spawnNumber; i++)
        {
            var x = Random.Range(-spawnRadius, spawnRadius);
            var z = Random.Range(-spawnRadius, spawnRadius);
            Instantiate(obstaclePrefab, new Vector3(x, 0.5f, z), Quaternion.identity);
        }
    }

}
