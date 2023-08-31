using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using Fusion;
using Projects.CarrySystem.Block.Scripts;
using Projects.Utility.Scripts;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    public class EditMapBlockBuilder : IBlockBuilder
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<BlockPresenterNet> _blockPresenterPrefabSpawner;

        public EditMapBlockBuilder(NetworkRunner runner)
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
                var girdPos = tmpMap.ToVector(i);
                var worldPos = GridConverter.GridPositionToWorldPosition(girdPos);
                
                // Presenterの生成
                var blockPresenterPrefab = _blockPresenterPrefabSpawner.Load();
                var blockPresenter =  _runner.Spawn(blockPresenterPrefab, worldPos, Quaternion.identity, PlayerRef.None);
                
                // BlockMonoDelegateの生成
                var getBlocks = tmpMap.GetSingleEntityList<IBlock>(i);
                var checkedBlocks = CheckBlocks(getBlocks);
                var blockControllerComponents = blockPresenter.GetComponentsInChildren<BlockControllerNet>();
                var blockInfos = blockControllerComponents.Select(c => c.Info).ToList();
                var blockMonoDelegate = new BlockMonoDelegate(checkedBlocks,blockInfos, blockPresenter);  // すべてのマスにBlockMonoDelegateを配置させる
                map.AddEntity(i, blockMonoDelegate);

                blockPresenters.Add(blockPresenter);
            }
            
            Debug.Log($"blockPresenters.Count : {blockPresenters.Count}");

            return (blockControllers, blockPresenters);

        }
        
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