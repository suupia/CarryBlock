﻿using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Gimmick.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Spawners.Interfaces;
using Fusion;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using Projects.CarrySystem.Gimmick.Scripts;
using Projects.CarrySystem.Item.Interfaces;
using Projects.CarrySystem.Item.Scripts;
using UnityEngine;
using VContainer;
#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    /// <summary>
    /// Spawn BlockPresenterPrefabs depending on the EntityGridMap data.
    /// </summary>
    public class EditMapBlockBuilder
    {
        readonly IEntityPresenterSpawner _entityPresenterSpawner;

        public EditMapBlockBuilder(IEntityPresenterSpawner entityPresenterSpawner)
        {
            _entityPresenterSpawner = entityPresenterSpawner;
        }


        // CarryBuilderと対応させてある。
        public (IReadOnlyList< BlockControllerNet>,IReadOnlyList<IEntityPresenter>) Build (EntityGridMap map)
        {
            var blockControllers = new List<BlockControllerNet>();
            var blockPresenters = new List<IEntityPresenter>();
            
            List<BlockMonoDelegate> blockMonoDelegates = new List<BlockMonoDelegate>();


            // BlockPresenterをスポーンさせる
            for (int i = 0; i < map.Length; i++)
            {
                var gridPos = map.ToVector(i);
                var worldPos = GridConverter.GridPositionToWorldPosition(gridPos);
                
                // Presenterの生成
                var entityPresenter = _entityPresenterSpawner.SpawnPrefab(worldPos, Quaternion.identity);

                // BlockMonoDelegateの生成
                var getBlocks = map.GetSingleEntityList<IBlock>(i);
                var checkedBlocks = CheckBlocks(getBlocks);
                var items = map.GetSingleEntityList<IItem>(i);
                var gimmicks = map.GetSingleEntityList<IGimmick>(i);

                // get blockInfos from blockController
                var blockControllerComponents = entityPresenter.GetMonoBehaviour.GetComponentsInChildren<BlockControllerNet>();
                var blockInfos = blockControllerComponents.Select(c => c.Info).ToList();
                // get itemInfos from itemController
                var itemControllerComponents = entityPresenter.GetMonoBehaviour.GetComponentsInChildren<ItemControllerNet>();
                var itemInfos = itemControllerComponents.Select(c => c.Info).ToList();
                // get gimmickInfos from gimmickController
                var gimmickControllerComponents = entityPresenter.GetMonoBehaviour.GetComponentsInChildren<GimmickControllerNet>();
                var gimmickInfos = gimmickControllerComponents.Select(c => c.Info).ToList();
                
                var blockMonoDelegate =
                    new BlockMonoDelegate(
                        gridPos, 
                        checkedBlocks, blockInfos,
                        items, itemInfos,
                        gimmicks, gimmickInfos,
                        entityPresenter); // すべてのマスにBlockMonoDelegateを配置させる
                blockMonoDelegates.Add(blockMonoDelegate);
                
                blockPresenters.Add(entityPresenter);
            }
            
            // MonoDelegateをmapに追加していることに注意
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
                Debug.LogError(
                    $"異なる種類のブロックが含まれています。　firstBlock.GetType() : {firstBlock.GetType()} {string.Join(",", blocks)}");
                return new List<IBlock>();
            }

            return blocks;
        }

    }
}