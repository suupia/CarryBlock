using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int spawnNumber;
    Collider shootRangeCollider;

    void Start()
    {


        for(int i = 0;i< spawnNumber; i++)
        {
            var x = Random.Range(-30, 30);
            var z = Random.Range(-30, 30);
            Instantiate(enemyPrefab, new Vector3(x, 0.5f, z),Quaternion.identity);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }
}
