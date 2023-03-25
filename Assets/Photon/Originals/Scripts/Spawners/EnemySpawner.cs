using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFusion
{

    public class EnemySpawner : SimulationBehaviour
    {
        [SerializeField] NetworkEnemy[] enemies;

        public void SpawnEnemy(int index)
        {
            var position = new Vector3(0, 1, 0);
            Runner.Spawn(enemies[index], position, Quaternion.identity, PlayerRef.None);
        }
    }
}

