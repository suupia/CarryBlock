using System;
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

        public BlockBuilder(NetworkRunner runner)
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
            
            //Debug
            for(int i = 0; i < tmpMap.GetLength(); i++)
            {
                var blocks = tmpMap.GetSingleEntityList<IBlock>(i);
                Debug.Log($"tmpMap blocks : {string.Join(",", blocks)}");
            }
            
            // BlockPresenterをスポーンさせる
            for (int i = 0; i < tmpMap.GetLength(); i++)
            {
                var girdPos = tmpMap.ToVector(i);
                var worldPos = GridConverter.GridPositionToWorldPosition(girdPos);
                var blockPresenterPrefab = _blockPresenterPrefabSpawner.Load();
                var blockPresenter =  _runner.Spawn(blockPresenterPrefab, worldPos, Quaternion.identity, PlayerRef.None);

                var blockControllerComponents = blockPresenter.GetComponentsInChildren<BlockControllerNet>();
                string debug = "blockControllerComponents :";
                for (int k = 0; k < blockControllerComponents.Length; k++)
                {
                    debug += " " + blockControllerComponents[k].GetType().Name + " ";
                }

                Debug.Log(debug);
                foreach (var blockController in blockControllerComponents)
                {
                    var getBlocks = tmpMap.GetSingleEntityList<IBlock>(i);
                    Debug.Log($"getBlocks : {string.Join(",", getBlocks)}");
                    var checkedBlocks = CheckBlocks(getBlocks);
                    if (checkedBlocks.Any())
                    {
                        Debug.Log($"BlockMonoDelegate Add");
                        var blockType =  checkedBlocks.First().GetType();
                        if (blockController.Info.BlockType == blockType)
                        {
                            var blockMonoDelegate = new BlockMonoDelegate(checkedBlocks);
                            blockController.Init(blockMonoDelegate);
                            map.AddEntity(i,blockMonoDelegate);
                        }

                    }
                    else
                    {
                        Debug.Log($"BlockMonoDelegate Not Add");
                    }

                }
                blockPresenters.Add(blockPresenter);
            }

            map = tmpMap;

            return (blockControllers, blockPresenters);

        }
        
        List<IBlock> CheckBlocks(List<IBlock> blocks)
        {
            if (!blocks.Any())
            {
                    Debug.Log($"IBlockが存在しません。{string.Join(",", blocks)}");
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