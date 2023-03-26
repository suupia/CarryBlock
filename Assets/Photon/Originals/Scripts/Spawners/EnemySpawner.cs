using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFusion
{

    public class EnemySpawner : SimulationBehaviour
    {
        [SerializeField] NetworkEnemy[] enemyPrefabs;


        public void SpawnEnemy(int index)
        {
            var position = new Vector3(0, 1, 0);
            Runner.Spawn(enemyPrefabs[index], position, Quaternion.identity, PlayerRef.None);
        }

        IEnumerator SimpleSpawner(int index, float interval)
        {
            while (true)
            {
                SpawnEnemy(index);
                yield return new WaitForSeconds(interval);
            }
        }

        public IEnumerator StartSimpleSpawner(int index, float interval)
        {
            var spawner = SimpleSpawner(index, interval);
            StartCoroutine(spawner);
            return spawner;
        }
    }
}

