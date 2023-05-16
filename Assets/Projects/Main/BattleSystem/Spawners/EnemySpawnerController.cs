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
        const string EnemySpawnerTransformTagName = "EnemySpawnerTransform";

        [SerializeField] List<Transform> spawnTransforms;

        List<EnemySpawner> _enemySpawners = new();

        public override void Spawned()
        {
            if (HasStateAuthority)
            {
                SetUpSpawnTransforms();
                SetUpEnemySpawners();
            }
        }

        void SetUpEnemySpawners()
        {
            _enemySpawners = spawnTransforms
                .Map(st => new EnemySpawner(
                    new EnemySpawner.EnemySpawnerRecord
                    {
                        GetCenter = () => st.position,
                        Runner = Runner,
                        SpawnRadius = st.GetComponent<SphereCollider>()?.radius ?? 1f,
                    }))
                .ToList();
        }

        void SetUpSpawnTransforms()
        {
            var transforms = GameObject
                .FindGameObjectsWithTag(EnemySpawnerTransformTagName)
                .Map(g => g.transform);
            
            spawnTransforms = spawnTransforms
                .Union(transforms)
                .ToList();
        }

        public void CancelSpawning()
        {
            _enemySpawners.ForEach(s => s.CancelSpawning());
        }

        public void StartSimpleSpawner(NetworkEnemyContainer networkEnemyContainer, bool stopBeforeSpawner = true)
        {
            if (_enemySpawners.Count == 0)
            {
                Debug.LogWarning("EnemySpawners is empty. No enemies will be spawned");    
            }
            
            if (stopBeforeSpawner)
            {
                CancelSpawning();
            }

            _enemySpawners.ForEach(s =>
            {
                var _ = s.StartSimpleSpawner(0, 5, networkEnemyContainer);
            });
        }
    }
}