using System.Collections.Generic;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Spawners.Interfaces;
using Projects.CarrySystem.Item.Interfaces;
using Projects.CarrySystem.Item.Scripts;
using UnityEngine;
using VContainer;
#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class EntityPresenterBuilder
    {
        readonly IEntityPresenterSpawner _entityPresenterSpawner;

        [Inject]
        public EntityPresenterBuilder(
            IEntityPresenterSpawner entityPresenterSpawner
        )
        {
            _entityPresenterSpawner = entityPresenterSpawner;
        }


        // CarryBuilderと対応させてある。
        public (IReadOnlyList<BlockControllerNet>, IReadOnlyList<IEntityPresenter>) Build(EntityGridMap map)
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
                
                // ItemControllerのInitを呼び出す
                var itemControllers =  entityPresenter.GetMonoBehaviour.GetComponentsInChildren<ItemControllerNet>();
                var items = map.GetSingleEntityList<IItem>(gridPos);
                foreach (var itemController in itemControllers)
                {
                    itemController.Init(items);
                }

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