using System;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Spawners;
using Fusion;
using Carry.Utility.Scripts;
using Carry.Utility;
using Carry.Utility.Interfaces;
using Projects.CarrySystem.Item.Interfaces;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class CarryBlockBuilder
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<EntityPresenterNet> _blockPresenterPrefabSpawner;

        public CarryBlockBuilder(NetworkRunner runner)
        {
            _runner = runner;
            _blockPresenterPrefabSpawner =
                new PrefabLoaderFromAddressable<EntityPresenterNet>("Prefabs/Map/BlockPresenter");
        }


        // CarryBuilderと対応させてある。
        public (IReadOnlyList< BlockControllerNet>,IReadOnlyList< EntityPresenterNet>) Build (ref EntityGridMap map)
        {
            var blockControllers = new List<BlockControllerNet>();
            var blockPresenters = new List<EntityPresenterNet>();

            // IBlockをIBlockMonoDelegateに置き換えたマップにする
            var tmpMap = map.CloneMap();  // tmpMapを見て、mapを変更する
            map.ClearMap();

            // BlockPresenterをスポーンさせる
            for (int i = 0; i < tmpMap.Length; i++)
            {
                var gridPos = tmpMap.ToVector(i);
                var worldPos = GridConverter.GridPositionToWorldPosition(gridPos);
                
                // Presenterの生成
                var blockPresenterPrefab = _blockPresenterPrefabSpawner.Load();
                var entityPresenter =  _runner.Spawn(blockPresenterPrefab, worldPos, Quaternion.identity, PlayerRef.None);
                
                // BlockMonoDelegateの生成
                var getBlocks = tmpMap.GetSingleEntityList<IBlock>(i);
                var checkedBlocks = CheckBlocks(getBlocks);
                var items = tmpMap.GetSingleEntityList<IItem>(i);
                var blockControllerComponents = entityPresenter.GetComponentsInChildren<BlockControllerNet>();
                var blockInfos = blockControllerComponents.Select(c => c.Info).ToList();
                var blockMonoDelegate = new BlockMonoDelegate(_runner, gridPos,checkedBlocks,blockInfos,items, entityPresenter);  // すべてのマスにBlockMonoDelegateを配置させる
                map.AddEntity(i, blockMonoDelegate);
                

                blockPresenters.Add(entityPresenter);
            }
            
            Debug.Log($"blockPresenters.Count : {blockPresenters.Count}");

            return (blockControllers, blockPresenters);

        }
        
        // Check if the blocks are the same type.
        List<IBlock> CheckBlocks(List<IBlock> blocks)
        {
            if (!blocks.Any())
            {
                // Debug.Log($"IBlockが存在しません。{string.Join(",", blocks)}");
                return new List<IBlock>();
            }
            var firstBlock = blocks.First();

            if (blocks.Any(block => block.GetType() != firstBlock.GetType()))
            {
                Debug.LogError($"異なる種類のブロックが含まれています。　firstBlock.GetType() : {firstBlock.GetType()} {string.Join(",", blocks)}");
                return new List<IBlock>();
            }
            
            return blocks;

        }

    }
}