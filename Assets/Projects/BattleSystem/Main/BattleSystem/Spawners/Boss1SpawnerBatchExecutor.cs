using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Main;
using UnityEngine;

namespace Nuts.Projects.BattleSystem.Main.BattleSystem.Spawners
{

    /// <summary>
    /// このクラスの責務は、EnemySpawnerをすべて管理するものとする
    /// Transformの取得は他のクラスが責務を負う
    /// 具体的には、すべてのEnemySpawnerに対して何かをしたいときに、このクラスが責任を負う
    /// </summary>
    public class Boss1SpawnersBatchExecutor
    {
        readonly List<Boss1Spawner> _bossSpawners;
        readonly StartSimpleSpawnerRecord _defaultRecord = new();

        public Boss1SpawnersBatchExecutor(NetworkRunner runner, SpawnerTransformContainer container)
        {
            _bossSpawners = container.Transforms
                .Select(st => new Boss1Spawner(runner, 
                    new Boss1Spawner.Boss1SpawnerRecord
                    {
                        GetCenter = () => st.position,
                        SpawnRadius = st.GetComponent<SphereCollider>()?.radius ?? 1f,
                    }))
                .ToList();
        }

        public void CancelSpawning()
        {
            _bossSpawners.ForEach(s => s.CancelSpawning());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="networkEnemyContainer"></param>
        /// <param name="stopBeforeSpawn"></param>
        /// <param name="startSimpleSpawnerDelegate">第一引数にindex, 第二引数にEnemySpawner, 戻り値にStartSimpleSpawnerRecordを返す関数</param>
        public void StartSimpleSpawner(
            Boss1Container networkEnemyContainer,
            bool stopBeforeSpawn = true,
            Func<int, Boss1Spawner, StartSimpleSpawnerRecord> startSimpleSpawnerDelegate = null)
        {
            if (_bossSpawners.Count == 0)
            {
                Debug.LogWarning("BossSpawners is empty. No bosses will be spawned");
            }

            if (stopBeforeSpawn)
            {
                CancelSpawning();
            }

            for (var i = 0; i < _bossSpawners.Count; i++)
            {
                var spawner = _bossSpawners[i];
                var record = startSimpleSpawnerDelegate?.Invoke(i, spawner) ?? _defaultRecord;
                var _ = spawner.StartSimpleSpawner(record.Index, record.Interval, networkEnemyContainer);
            }
        }
    }
}