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
        public void GenerateMap(NetworkRunner runner, EntityGridMap map)
        {
            var groundSpawner = new GroundSpawner(runner);
            var rockSpawner = new RockSpawner(runner);
            
            for (int i = 0; i < map.GetLength(); i++)
            {
                var girdPos = map.GetVectorFromIndex(i);
                var worldPos = GridConverter.GridPositionToWorldPosition(girdPos);
                if (map.GetSingleEntity<Ground>(i) != null)
                {
                    groundSpawner.SpawnPrefab(worldPos, Quaternion.identity);
                }
                if (map.GetSingleEntity<Rock>(i) != null)
                {
                    rockSpawner.SpawnPrefab(worldPos, Quaternion.identity);
                }

            }
        }
        
    }
}