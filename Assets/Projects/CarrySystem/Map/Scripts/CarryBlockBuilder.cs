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
using Projects.Utility.Scripts;
using Projects.Utility;
using Projects.Utility.Interfaces;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class CarryBlockBuilder :  IBlockBuilder
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<BlockPresenterNet> _blockPresenterPrefabSpawner;

        public CarryBlockBuilder(NetworkRunner runner)
        {
            _runner = runner;
            _blockPresenterPrefabSpawner =
                new PrefabLoaderFromAddressable<BlockPresenterNet>("Prefabs/Map/BlockPresenter");
        }


        // CarryBuilderと対応させてある。
        public (IReadOnlyList< BlockControllerNet>,IReadOnlyList< BlockPresenterNet>) Build (ref EntityGridMap map)
        {
            var blockControllers = new List<BlockControllerNet>();
            var blockPresenters = new List<BlockPresenterNet>();

            // IBlockをIBlockMonoDelegateに置き換えたマップにする
            var tmpMap = map.CloneMap();  // tmpMapを見て、mapを変更する
            map.ClearMap();

            // BlockPresenterをスポーンさせる
            for (int i = 0; i < tmpMap.GetLength(); i++)
            {
                var gridPos = tmpMap.ToVector(i);
                var worldPos = GridConverter.GridPositionToWorldPosition(gridPos);
                
                // Presenterの生成
                var blockPresenterPrefab = _blockPresenterPrefabSpawner.Load();
                var blockPresenter =  _runner.Spawn(blockPresenterPrefab, worldPos, Quaternion.identity, PlayerRef.None);
                
                // BlockMonoDelegateの生成
                var getBlocks = tmpMap.GetSingleEntityList<ICarriableBlock>(i);
                var checkedBlocks = CheckBlocks(getBlocks);
                var blockControllerComponents = blockPresenter.GetComponentsInChildren<BlockControllerNet>();
                var blockInfos = blockControllerComponents.Select(c => c.Info).ToList();
                var blockMonoDelegate = new BlockMonoDelegate(gridPos,checkedBlocks,blockInfos, blockPresenter);  // すべてのマスにBlockMonoDelegateを配置させる
                map.AddEntity(i, blockMonoDelegate);

                blockPresenters.Add(blockPresenter);
            }
            
            Debug.Log($"blockPresenters.Count : {blockPresenters.Count}");

            return (blockControllers, blockPresenters);

        }
        
        List<ICarriableBlock> CheckBlocks(List<ICarriableBlock> blocks)
        {
            if (!blocks.Any())
            {
                // Debug.Log($"IBlockが存在しません。{string.Join(",", blocks)}");
                return new List<ICarriableBlock>();
            }
            var firstBlock = blocks.First();

            if (blocks.Any(block => block.GetType() != firstBlock.GetType()))
            {
                Debug.LogError($"異なる種類のブロックが含まれています。　firstBlock.GetType() : {firstBlock.GetType()} {string.Join(",", blocks)}");
                return new List<ICarriableBlock>();
            }
            
            return blocks;

        }
    }
}