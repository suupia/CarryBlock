using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Main;
using UnityEngine;

namespace Nuts.BattleSystem.Spawners.Scripts
{
    public record StartSimpleSpawnerRecord
    {
        public int Index = 0;
        public float Interval = 5f;
    }

    /// <summary>
    /// このクラスの責務は、EnemySpawnerをすべて管理するものとする
    /// Transformの取得は他のクラスが責務を負う
    /// 具体的には、すべてのEnemySpawnerに対して何かをしたいときに、このクラスが責任を負う
    /// </summary>
    public class EnemySpawnersBatchExecutor
    {
        readonly List<EnemySpawner> _enemySpawners;
        readonly StartSimpleSpawnerRecord _defaultRecord = new();

        public EnemySpawnersBatchExecutor(NetworkRunner runner, SpawnerTransformContainer container)
        {
            _enemySpawners = container.Transforms
                .Select(st => new EnemySpawner(runner, 
                    new EnemySpawner.EnemySpawnerRecord
                    {
                        GetCenter = () => st.position,
                        SpawnRadius = st.GetComponent<SphereCollider>()?.radius ?? 1f,
                    }))
                .ToList();
        }

        public void CancelSpawning()
        {
            _enemySpawners.ForEach(s => s.CancelSpawning());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="networkEnemyContainer"></param>
        /// <param name="stopBeforeSpawn"></param>
        /// <param name="startSimpleSpawnerDelegate">第一引数にindex, 第二引数にEnemySpawner, 戻り値にStartSimpleSpawnerRecordを返す関数</param>
        public void StartSimpleSpawner(
            NetworkEnemyContainer networkEnemyContainer,
            bool stopBeforeSpawn = true,
            Func<int, EnemySpawner, StartSimpleSpawnerRecord> startSimpleSpawnerDelegate = null)
        {
            if (_enemySpawners.Count == 0)
            {
                Debug.LogWarning("EnemySpawners is empty. No enemies will be spawned");
            }

            if (stopBeforeSpawn)
            {
                CancelSpawning();
            }

            for (var i = 0; i < _enemySpawners.Count; i++)
            {
                var spawner = _enemySpawners[i];
                var record = startSimpleSpawnerDelegate?.Invoke(i, spawner) ?? _defaultRecord;
                var _ = spawner.StartSimpleSpawner(record.Index, record.Interval, networkEnemyContainer);
            }
        }
    }
}