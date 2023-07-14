using System.Collections.Generic;
using Carry.CarrySystem.Spawners;
using Fusion;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    public class TilePresenterBuilder
    {
        [Inject] NetworkRunner _runner;

        public IEnumerable<TilePresenter_Net> Build(EntityGridMap map)
        {
            // TilePresenterをスポーンさせる
            var tilePresenterSpawner = new TilePresenterSpawner(_runner);
            var tilePresenters = new List<TilePresenter_Net>();

            for (int i = 0; i < map.GetLength(); i++)
            {
                var girdPos = map.GetVectorFromIndex(i);
                var worldPos = GridConverter.GridPositionToWorldPosition(girdPos);
                var tilePresenter = tilePresenterSpawner.SpawnPrefab(worldPos, Quaternion.identity);
                tilePresenters.Add(tilePresenter);
            }

            return tilePresenters;
        }
        
        
    }
}