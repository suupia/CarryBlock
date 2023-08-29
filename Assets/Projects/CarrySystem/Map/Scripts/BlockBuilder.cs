using System;
using System.Collections.Generic;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Spawners;
using Fusion;
using Projects.CarrySystem.Block.Scripts;
using Projects.Utility.Scripts;
using Projects.Utilty;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class BlockBuilder
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<BlockPresenterNet> _blockPresenterPrefabSpawner;
        readonly BlockMonoDelegateDictionary _blockMonoDelegateDictionary;

        public BlockBuilder(NetworkRunner runner , BlockMonoDelegateDictionary blockMonoDelegateDictionary)
        {
            _runner = runner;
            _blockPresenterPrefabSpawner =
                new PrefabLoaderFromAddressable<BlockPresenterNet>("Prefabs/Map/BlockPresenter");
            _blockMonoDelegateDictionary = blockMonoDelegateDictionary;
        }


        // CarryBuilderと対応させてある。
        public (IReadOnlyList< BlockControllerNet>,IReadOnlyList< BlockPresenterNet>) Build(EntityGridMap map)
        {
            var blockControllers = new List<BlockControllerNet>();
            var blockPresenters = new List<BlockPresenterNet>();
            
            // BlockPresenterをスポーンさせる
            for (int i = 0; i < map.GetLength(); i++)
            {
                var girdPos = map.ToVector(i);
                var worldPos = GridConverter.GridPositionToWorldPosition(girdPos);
                var blockPresenterPrefab = _blockPresenterPrefabSpawner.Load();
                var blockPresenter =  _runner.Spawn(blockPresenterPrefab, worldPos, Quaternion.identity, PlayerRef.None);

                var blockControllerComponents = blockPresenter.GetComponentsInChildren<BlockControllerNet>();
                foreach (var blockController in blockControllerComponents)
                {
                    // ToDo:  重複がないことを判定する
                    var blocks = map.GetSingleEntityList<IBlock>(i);
                    var blockMonoDelegate = default(BlockMonoDelegate);  // ToDo: BlockMonoDelegateを作成する
                    blockController.Init(blockMonoDelegate);
                }
                blockPresenters.Add(blockPresenter);
            }

            return (blockControllers, blockPresenters);

        }

    }
}