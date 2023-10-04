using System;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.CarriableBlock.Scripts;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.Utility;
using Projects.CarrySystem.Item.Scripts;
using UnityEngine;
using VContainer;

#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class EntityGridMapBuilder : IEntityGridMapBuilder
    {
        public EntityGridMap BuildEntityGridMap(EntityGridMapData gridMapData)
        {
            var map = new EntityGridMap(gridMapData.width, gridMapData.height);
            for (int i = 0; i < map.Length; i++)
            {
                AddEntityFromRecord<Ground, GroundRecord, Ground.Kind>(gridMapData.groundRecords, () => gridMapData.groundRecords?.Length ??0 ,(record) => record.kinds, Ground.Kind.None, map, i );
                AddEntityFromRecord<BasicBlock, BasicBlockRecord, BasicBlock.Kind>(gridMapData.basicBlockRecords, () =>gridMapData.basicBlockRecords?.Length?? 0, (record) => record.kinds, BasicBlock.Kind.None, map, i );
                AddEntityFromRecord<UnmovableBlock, RockRecord, UnmovableBlock.Kind>(gridMapData.rockRecords, () => gridMapData.rockRecords?.Length?? 0,(record) => record.kinds, UnmovableBlock.Kind.None, map, i );
                AddEntityFromRecord<HeavyBlock, HeavyBlockRecord, HeavyBlock.Kind>(gridMapData.heavyBlockRecords, () => gridMapData.heavyBlockRecords?.Length?? 0, (record) => record.kinds, HeavyBlock.Kind.None, map, i );
                AddEntityFromRecord<FragileBlock, FragileBlockRecord, FragileBlock.Kind>(gridMapData.fragileBlockRecords, () => gridMapData.fragileBlockRecords?.Length ?? 0, (record) => record.kinds, FragileBlock.Kind.None, map, i );
                AddEntityFromRecord<Cannon, CannonBlockRecord, Cannon.Kind>(gridMapData.cannonBlockRecords, () => gridMapData.cannonBlockRecords?.Length ?? 0, (record) => record.kinds, Cannon.Kind.None, map, i );
               
                // TreasureCoin
                if (gridMapData.treasureCoinRecords != null)
                {
                    var kinds = gridMapData.treasureCoinRecords[i].kinds;
                    foreach (var kind in kinds)
                    {
                        if (!kind.Equals(TreasureCoin.Kind.None))
                        {
                            // This will create a new entity
                            var item = new TreasureCoin(kind, map.ToVector(i), map,new TreasureCoinCounter());
                            map.AddEntity(i, item);
                        }
                    }
                }
            }

            return map;
        }
        
        void AddEntityFromRecord<TEntity,TRecord,TKind>(TRecord[]? records,Func<int> getRecordLength, Func<TRecord,TKind[]> funcKins,TKind noneValue, EntityGridMap map, int index) 
            where TEntity : IEntity
            where TRecord : class 
            where TKind : Enum
        {
            
            if (records != null)
            {
                var kinds = funcKins(records[index]);
                if (getRecordLength() == map.Length)
                {
                    foreach (var kind in kinds)
                    {
                        if (!kind.Equals(noneValue))
                        {
                            // This will create a new entity
                            var entity = Generic.Construct<TEntity, TKind, Vector2Int>(kind, map.ToVector(index));
                            map.AddEntity(index, entity);
                        }
                    }
                }
                else
                {
                    Debug.LogError($"{typeof(TKind).Name} Records.Length is not equal to map.GetLength()!");
                }
            }
            else
            {
                Debug.LogWarning($"{typeof(TKind).Name} Records is not initialized properly!");
            }
        }
    }
}