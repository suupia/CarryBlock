using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

namespace Main
{
    /// <summary>
    /// EnemySpawnerTransformタグがついているか、
    /// ゲーム開始前にspawnerTransformsにアタッチされたTransformを基準として
    /// EnemySpawnerに従ってSpawnする
    /// このクラスの責務は、EnemySpawnerをすべて管理するものとする
    /// 具体的には、すべてのEnemySpawnerに対して何かをしたいときに、このクラスが責任を負う
    /// </summary>
    public class EnemySpawnerController : NetworkBehaviour
    {
        private const string EnemySpawnerTransform = "EnemySpawnerTransform";

        [SerializeField] private List<Transform> spawnTransforms;

        private List<EnemySpawner> _enemySpawners = new();
        
        public override void Spawned()
        {
            if (HasStateAuthority)
            {
                SetUpSpawnTransforms();
                _enemySpawners = spawnTransforms
                    .Map(st => new EnemySpawner(
                        new EnemySpawner.EnemySpawnerRecord
                        {
                            GetOffset = () => st.position,
                            Runner = Runner,
                            SpawnRadius = st.GetComponent<SphereCollider>()?.radius ?? 1f,
                        }))
                    .ToList();
            }
        }

        void SetUpSpawnTransforms()
        {
            var st = GameObject
                .FindGameObjectsWithTag(EnemySpawnerTransform)
                .Map(g => g.transform);
            spawnTransforms = spawnTransforms.Union(st).ToList();
        }

        public void CancelSpawning()
        {
            _enemySpawners.ForEach(s => s.CancelSpawning());
        }

        public void StartSimpleSpawner(NetworkEnemyContainer networkEnemyContainer, bool stopBeforeSpawner = true)
        {
            if (stopBeforeSpawner)
            {
                CancelSpawning();
            }
            _enemySpawners.ForEach(s => s.StartSimpleSpawner(0, 5, networkEnemyContainer));
        }
    }
}
