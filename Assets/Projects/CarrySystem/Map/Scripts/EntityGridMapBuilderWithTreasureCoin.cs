using System;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.CarriableBlock.Scripts;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.Utility;
using Projects.CarrySystem.Gimmick.Scripts;
using Projects.CarrySystem.Item.Scripts;
using UnityEngine;
using VContainer;
#nullable enable
namespace Carry.CarrySystem.Map.Scripts
{
    public class EntityGridMapBuilderWithTreasureCoin : IEntityGridMapBuilder
    {
        readonly IEntityGridMapBuilder _entityGridMapBuilder;
        readonly TreasureCoinCounter _treasureCoinCounter;
        
        [Inject]
        public EntityGridMapBuilderWithTreasureCoin(IEntityGridMapBuilder entityGridMapBuilder, TreasureCoinCounter treasureCoinCounter)
        {
            _entityGridMapBuilder = entityGridMapBuilder;
            _treasureCoinCounter = treasureCoinCounter;
        }
        public EntityGridMap BuildEntityGridMap(EntityGridMapData gridMapData)
        {
            var map = _entityGridMapBuilder.BuildEntityGridMap(gridMapData);
            for (int i = 0; i < map.Length; i++)
            {
                // TreasureCoin
                if (gridMapData.treasureCoinRecords != null)
                {
                    var kinds = gridMapData.treasureCoinRecords[i].kinds;
                    foreach (var kind in kinds)
                    {
                        if (!kind.Equals(TreasureCoin.Kind.None))
                        {
                            // This will create a new entity
                            var item = new TreasureCoin(kind, map.ToVector(i),map,_treasureCoinCounter);
                            map.AddEntity(i, item);
                        }
                    }
                }
            }

            return map;
        }
        
    }
}