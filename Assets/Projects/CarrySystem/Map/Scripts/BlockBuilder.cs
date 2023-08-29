﻿using System;
using System.Collections.Generic;
using System.Linq;
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
                    var blocks = map.GetSingleEntityList<IBlock>(i);
                    var block = DecideOneBlock(blocks);
                    if (block != null)
                    {
                        var blockMonoDelegate = new BlockMonoDelegate(block);
                        blockController.Init(blockMonoDelegate);
                        _blockMonoDelegateDictionary.Add(block, blockMonoDelegate);
                    }

                }
                blockPresenters.Add(blockPresenter);
            }

            return (blockControllers, blockPresenters);

        }
        
        IBlock? DecideOneBlock(List<IBlock> blocks)
        {
            if (!blocks.Any())
            {
                Debug.Log($"IBlockが存在しません。{string.Join(",", blocks)}");
                return null!;
            }
            var firstBlock = blocks.First();
            
            if (blocks.All(block => block.GetType() == firstBlock.GetType()))
            {
                return firstBlock;
            }
            else
            {
                Debug.LogError($"異なる種類のブロックが含まれています。　firstBlock.GetType() : {firstBlock.GetType()} {string.Join(",", blocks)}");
                return null!;
            }
        }
    }
}