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
            var tilePresenterSpawner = new TilePresenterSpawner(runner);

            for (int i = 0; i < map.GetLength(); i++)
            {
                var girdPos = map.GetVectorFromIndex(i);
                var worldPos = GridConverter.GridPositionToWorldPosition(girdPos);
                var tilePresenter = tilePresenterSpawner.SpawnPrefab(worldPos, Quaternion.identity);
                var presentData = tilePresenter.PresentDataRef;
                if (map.GetSingleEntity<Ground>(i) != null)
                {
                    presentData.isGroundActive = true;
                    tilePresenter.SetEntityActiveData(map.GetSingleEntity<Ground>(i), true);
                }
                if (map.GetSingleEntity<Rock>(i) != null)
                {
                    presentData.isRockActive = true;
                    tilePresenter.SetEntityActiveData(map.GetSingleEntity<Rock>(i), true);
                }
                // tilePresenter.SetPresentData(presentData);
                
                // mapにTilePresenterを登録
                map.RegisterTilePresenter(tilePresenter, i);
            }
        }
        
    }
}