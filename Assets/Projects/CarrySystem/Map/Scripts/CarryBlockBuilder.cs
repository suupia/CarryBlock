using System.Collections.Generic;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.Utility.Scripts;
using Carry.Utility.Interfaces;
using Projects.CarrySystem.Item.Interfaces;
using Projects.CarrySystem.Item.Scripts;
using UnityEngine;
using VContainer;
using Fusion;
#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class CarryBlockBuilder
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<EntityPresenterNet> _blockPresenterPrefabSpawner;

        [Inject]
        public CarryBlockBuilder(
            NetworkRunner runner
        )
        {
            _runner = runner;
            _blockPresenterPrefabSpawner =
                new PrefabLoaderFromAddressable<EntityPresenterNet>("Prefabs/Map/EntityPresenter");
        }


        // CarryBuilderと対応させてある。
        public (IReadOnlyList<BlockControllerNet>, IReadOnlyList<IEntityPresenter>) Build(EntityGridMap map)
        {
            var blockControllers = new List<BlockControllerNet>();
            var blockPresenters = new List<IEntityPresenter>();

            List<BlockMonoDelegate> blockMonoDelegates = new List<BlockMonoDelegate>();

            var blockPresenterPrefab = _blockPresenterPrefabSpawner.Load();

            // BlockPresenterをスポーンさせる
            for (int i = 0; i < map.Length; i++)
            {
                var gridPos = map.ToVector(i);
                var worldPos = GridConverter.GridPositionToWorldPosition(gridPos);

                // Presenterの生成
                var entityPresenter = _runner.Spawn(blockPresenterPrefab, worldPos, Quaternion.identity, PlayerRef.None,
                    (runner, networkObj) =>
                    {
                        var itemControllers = networkObj.GetComponentsInChildren<ItemControllerNet>();
                        var items = map.GetSingleEntityList<IItem>(gridPos);
                        foreach (var itemController in itemControllers)
                        {
                            itemController.Init(items);
                        }
                    });

                var blockMonoDelegate =
                    new BlockMonoDelegate(
                        map,
                        gridPos,
                        entityPresenter); // すべてのマスにBlockMonoDelegateを配置させる
                blockMonoDelegates.Add(blockMonoDelegate);

                blockPresenters.Add(entityPresenter);
            }

            // MonoDelegateをmapに追加していることに注意
            for (int i = 0; i < map.Length; i++)
            {
                map.AddEntity(i, blockMonoDelegates[i]);
            }
            

            return (blockControllers, blockPresenters);
        }


    }
}