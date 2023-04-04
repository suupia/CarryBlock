using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyFusion
{

    public class EnemySpawner : SimulationBehaviour
    {
        [SerializeField] NetworkEnemy[] enemyPrefabs;

        List<NetworkEnemy> enemies = new();

        public NetworkEnemy[] Enemies => enemies.ToArray();
        public int MaxEnemyCount { get; set; } = 128;

        void SpawnEnemy(int index)
        {
            if (MaxEnemyCount > enemies.Count)
            {
                var position = new Vector3(0, 1, 0);
                var no = Runner.Spawn(enemyPrefabs[index], position, Quaternion.identity, PlayerRef.None);
                var enemy = no.GetComponent<NetworkEnemy>();
                enemy.OnDespawn += () => enemies.Remove(enemy);
                enemies.Add(enemy);
            }
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

