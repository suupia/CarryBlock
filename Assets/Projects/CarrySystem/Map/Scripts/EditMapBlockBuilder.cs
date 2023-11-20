using System.Collections.Generic;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Spawners.Interfaces;
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

        [Inject]
        public EditMapBlockBuilder(
            IEntityPresenterSpawner entityPresenterSpawner
            )
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