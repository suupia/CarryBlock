using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Spawners;
using UnityEngine;
using Fusion;
using Nuts.Utility.Scripts;
#nullable  enable

namespace Carry.CarrySystem.Map.Scripts
{
    /// <summary>
    /// シーン上にマップを生成するクラス
    /// ホストで呼ばれることを想定している
    /// </summary>
    public class MapGenerator
    {
        NetworkRunner _runner;

        readonly GridConverter _gridConverter;
        
        // Spawner
        readonly GroundSpawner _groundSpawner;
        readonly RockSpawner _rockSpawner;
        public  MapGenerator(NetworkRunner runner)
        {
            _runner = runner;
            
            _gridConverter = new GridConverter(1, 1);
            _groundSpawner = new GroundSpawner(_runner);
            _rockSpawner = new RockSpawner(_runner);
            
            var gridMapGenerator = new EntityGridMapGenerator();
            var entityGridMap =   gridMapGenerator.GenerateEntityGridMap(0); // indexはとりあえず0にしておく

            SetupMap(entityGridMap);
        }

        void SetupMap(EntityGridMap map)
        {
            for (int i = 0; i < map.GetLength(); i++)
            {
                var girdPos = map.GetVectorFromIndex(i);
                var worldPos = _gridConverter.GridPositionToWorldPosition(girdPos);
                if (map.GetSingleEntity<Ground>(i) != null)
                {
                    _groundSpawner.SpawnPrefab(worldPos, Quaternion.identity);
                }
                if (map.GetSingleEntity<Rock>(i) != null)
                {
                    _rockSpawner.SpawnPrefab(worldPos, Quaternion.identity);
                }

            }
        }
        
    }
}