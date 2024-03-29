﻿using System;
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
    public class EntityGridMapBuilderLeaf : IEntityGridMapBuilder
    { 
        public EntityGridMapBuilderLeaf()
        {
        }
        public EntityGridMap BuildEntityGridMap(EntityGridMapData gridMapData)
        {
            var coordinate = new SquareGridCoordinate(gridMapData.width, gridMapData.height);
            var map = new EntityGridMap(coordinate);
            for (int i = 0; i < map.Length; i++)
            {
                AddEntityFromRecord<Ground, GroundRecord, Ground.Kind>(gridMapData.groundRecords, () => gridMapData.groundRecords?.Length ??0 ,(record) => record.kinds, Ground.Kind.None, map, i );
                AddEntityFromRecord<BasicBlock, BasicBlockRecord, BasicBlock.Kind>(gridMapData.basicBlockRecords, () =>gridMapData.basicBlockRecords?.Length?? 0, (record) => record.kinds, BasicBlock.Kind.None, map, i );
                AddEntityFromRecord<UnmovableBlock, RockRecord, UnmovableBlock.Kind>(gridMapData.rockRecords, () => gridMapData.rockRecords?.Length?? 0,(record) => record.kinds, UnmovableBlock.Kind.None, map, i );
                AddEntityFromRecord<HeavyBlock, HeavyBlockRecord, HeavyBlock.Kind>(gridMapData.heavyBlockRecords, () => gridMapData.heavyBlockRecords?.Length?? 0, (record) => record.kinds, HeavyBlock.Kind.None, map, i );
                AddEntityFromRecord<FragileBlock, FragileBlockRecord, FragileBlock.Kind>(gridMapData.fragileBlockRecords, () => gridMapData.fragileBlockRecords?.Length ?? 0, (record) => record.kinds, FragileBlock.Kind.None, map, i );
                AddEntityFromRecord<CannonBlock, CannonBlockRecord, CannonBlock.Kind>(gridMapData.cannonBlockRecords, () => gridMapData.cannonBlockRecords?.Length ?? 0, (record) => record.kinds, CannonBlock.Kind.None, map, i );
                AddEntityFromRecord<ConfusionBlock, ConfusionBlockRecord, ConfusionBlock.Kind>(gridMapData.confusionBlockRecords, () => gridMapData.confusionBlockRecords?.Length ?? 0, (record) => record.kinds, ConfusionBlock.Kind.None, map, i );
                AddEntityFromRecord<SpikeGimmick, SpikeGimmickRecord, SpikeGimmick.Kind>(gridMapData.spikeGimmickRecords, () =>gridMapData.spikeGimmickRecords?.Length?? 0, (record) => record.kinds, SpikeGimmick.Kind.None, map, i );
                // todo : EmptyTreasureCoinを生成したいけど、プレゼンターの判定がうまくできなくなる。。。

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
                            var entity = Generic.Construct<TEntity, TKind>(kind);
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