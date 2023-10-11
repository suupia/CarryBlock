using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Fusion;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using UnityEngine;
using VContainer;
#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{

    public class LocalEditMapBlockBuilder
    {
        readonly IPrefabLoader<EntityPresenterNet> _blockPresenterPrefabSpawner;

        public LocalEditMapBlockBuilder()
        { 
            _blockPresenterPrefabSpawner =
                new PrefabLoaderFromAddressable<EntityPresenterNet>("Prefabs/Map/LocalEntityPresenter");
        }


        // CarryBuilderと対応させてある。
        public (IReadOnlyList< BlockControllerNet>,IReadOnlyList< EntityPresenterNet>) Build (ref EntityGridMap map)
        {
            var blockControllers = new List<BlockControllerNet>();
            var blockPresenters = new List<EntityPresenterNet>();

            // BlockPresenterをスポーンさせる
            for (int i = 0; i < map.Length; i++)
            {
                var girdPos = map.ToVector(i);
                var worldPos = GridConverter.GridPositionToWorldPosition(girdPos);
                
                // Presenterの生成
                var blockPresenterPrefab = _blockPresenterPrefabSpawner.Load();
                // var blockPresenter =  _runner.Spawn(blockPresenterPrefab, worldPos, Quaternion.identity, PlayerRef.None);
                var blockPresenter = UnityEngine.Object.Instantiate(blockPresenterPrefab, worldPos, Quaternion.identity);
                

                blockPresenters.Add(blockPresenter);
            }
            
            Debug.Log($"blockPresenters.Count : {blockPresenters.Count}");

            return (blockControllers, blockPresenters);

        }

    }
}