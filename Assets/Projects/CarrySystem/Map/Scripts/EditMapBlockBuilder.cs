using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Spawners.Interfaces;
using Fusion;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
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

            // BlockPresenterをスポーンさせる
            for (int i = 0; i < map.Length; i++)
            {
                var girdPos = map.ToVector(i);
                var worldPos = GridConverter.GridPositionToWorldPosition(girdPos);
                
                // Presenterの生成
                var entityPresenter = _entityPresenterSpawner.SpawnPrefab(worldPos, Quaternion.identity);

                blockPresenters.Add(entityPresenter);
            }
            
            Debug.Log($"blockPresenters.Count : {blockPresenters.Count}");

            return (blockControllers, blockPresenters);

        }

    }
}