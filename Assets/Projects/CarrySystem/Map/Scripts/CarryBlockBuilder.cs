﻿using System;
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
using Projects.CarrySystem.Item.Scripts;
using UnityEngine;
using VContainer;

#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class CarryBlockBuilder
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<EntityPresenterNet> _blockPresenterPrefabSpawner;

        [Inject]
        public CarryBlockBuilder(NetworkRunner runner)
        {
            _runner = runner;
            _blockPresenterPrefabSpawner =
                new PrefabLoaderFromAddressable<EntityPresenterNet>("Prefabs/Map/BlockPresenter");
        }


        // CarryBuilderと対応させてある。
        public (IReadOnlyList< BlockControllerNet>,IReadOnlyList< EntityPresenterNet>) Build (EntityGridMap map)
        {
            var blockControllers = new List<BlockControllerNet>();
            var blockPresenters = new List<EntityPresenterNet>();
            

            // IBlockをIBlockMonoDelegateに置き換えたマップにする
             // var tmpMap = map.CloneMap();  // tmpMapを見て、mapを変更する

             List<BlockMonoDelegate> blockMonoDelegates = new List<BlockMonoDelegate>();
             
             var blockPresenterPrefab = _blockPresenterPrefabSpawner.Load();


            // BlockPresenterをスポーンさせる
            for (int i = 0; i < map.Length; i++)
            {
                var gridPos = map.ToVector(i);
                var worldPos = GridConverter.GridPositionToWorldPosition(gridPos);
                
                // Presenterの生成
                var tmpMap = map;
                var entityPresenter =  _runner.Spawn(blockPresenterPrefab, worldPos, Quaternion.identity, PlayerRef.None,
                    (runner, networkObj) =>
                    {
                       var itemControllers = networkObj.GetComponentsInChildren<ItemControllerNet>();
                       var items = tmpMap.GetSingleEntityList<IItem>(gridPos);
                       if(items.Count() != 0) Debug.Log($"items.Count() : {items.Count()}");
                       foreach (var itemController in itemControllers)
                       {
                           itemController.Init(items);
                       }
                    });
                
                // BlockMonoDelegateの生成
                var getBlocks = map.GetSingleEntityList<IBlock>(i);
                var checkedBlocks = CheckBlocks(getBlocks);
                var items = map.GetSingleEntityList<IItem>(i);
                // get blockInfos from blockController
                var blockControllerComponents = entityPresenter.GetComponentsInChildren<BlockControllerNet>();
                var blockInfos = blockControllerComponents.Select(c => c.Info).ToList();
                // get itemInfos from blockController
                var itemControllerComponents = entityPresenter.GetComponentsInChildren<ItemControllerNet>();
                var itemInfos = itemControllerComponents.Select(c => c.Info).ToList();
                var blockMonoDelegate = new BlockMonoDelegate(_runner, gridPos,checkedBlocks,blockInfos,items,itemInfos, entityPresenter);  // すべてのマスにBlockMonoDelegateを配置させる
                // map.AddEntity(i, blockMonoDelegate);
                blockMonoDelegates.Add(blockMonoDelegate);
                

                blockPresenters.Add(entityPresenter);
            }

            for (int i = 0; i < map.Length; i++)
            {
                map.AddEntity(i, blockMonoDelegates[i]);
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