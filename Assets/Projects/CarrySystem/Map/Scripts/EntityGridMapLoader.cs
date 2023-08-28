using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Spawners;
using Projects.Utilty;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class EntityGridMapLoader
    {
        public EntityGridMap LoadEntityGridMap(MapKey key, int mapDataIndex)
        {
            var gridMapData = Load(key, mapDataIndex);
            return BuildEntityGridMap(gridMapData);
        }

        public EntityGridMap LoadDefaultEntityGridMap()
        {
            var defaultGridMapData = LoadDefault();
            return BuildEntityGridMap(defaultGridMapData);
        }

        EntityGridMapData LoadDefault()
        {
            EntityGridMapData entityGridMapData;

            string filePath = EntityGridMapFileUtility.GetDefaultFilePath(); // このパスには白紙のマップデータを必ず置いておく

            using (StreamReader streamReader = new StreamReader(filePath))
            {
                string data = streamReader.ReadToEnd();
                streamReader.Close();
                entityGridMapData = JsonUtility.FromJson<EntityGridMapData>(data);
            }
            
            Debug.Log($"Complete Load DefaultMapData\nfilePath:{filePath}");

            return entityGridMapData;
        }

        EntityGridMapData Load(MapKey key, int mapDataIndex)
        {
            EntityGridMapData entityGridMapData;
            string filePath = EntityGridMapFileUtility.GetFilePath(key, mapDataIndex);

            if (!EntityGridMapFileUtility.IsExitFile(key, mapDataIndex))
            {
                Debug.LogWarning($"パス:{filePath}にjsonファイルが存在しないためデフォルトのデータを読み込みます");
                filePath = EntityGridMapFileUtility.GetDefaultFilePath(); // このパスには白紙のマップデータを必ず置いておく
            }

            using (StreamReader streamReader = new StreamReader(filePath))
            {
                string data = streamReader.ReadToEnd();
                streamReader.Close();
                entityGridMapData = JsonUtility.FromJson<EntityGridMapData>(data);
            }

            Debug.Log($"Complete Load MapData:{key}_{mapDataIndex}\nfilePath:{filePath}");

            return entityGridMapData;
        }

        EntityGridMap BuildEntityGridMap(EntityGridMapData gridMapData)
        {
            var map = new EntityGridMap(gridMapData.width, gridMapData.height);
            for (int i = 0; i < map.GetLength(); i++)
            {
                // ProcessRecord(gridMapData.groundRecords, map, i, Ground.Kind.None, (kind, vector) => new Ground(kind, vector));
                // ProcessRecord(gridMapData.basicBlockRecords, map, i, BasicBlock.Kind.None, (kind, vector) => new BasicBlock(kind, vector));
                // ProcessRecord(gridMapData.rockRecords, map, i, UnmovableBlock.Kind.None, (kind, vector) => new UnmovableBlock(kind, vector));
                // ProcessRecord(gridMapData.heavyBlockRecords, map, i, HeavyBlock.Kind.None, (kind, vector) => new HeavyBlock(kind, vector));
                // ProcessRecord(gridMapData.fragileBlockRecords, map, i, FragileBlock.Kind.None, (kind, vector) => new FragileBlock(kind, vector));
                //
                AddEntityFromRecord<Ground, GroundRecord, Ground.Kind>(gridMapData.groundRecords[i], () => gridMapData.groundRecords.Length ,(record) => record.kinds, Ground.Kind.None, map, i );
                AddEntityFromRecord<BasicBlock, BasicBlockRecord, BasicBlock.Kind>(gridMapData.basicBlockRecords[i], () =>gridMapData.basicBlockRecords.Length, (record) => record.kinds, BasicBlock.Kind.None, map, i );
                AddEntityFromRecord<UnmovableBlock, RockRecord, UnmovableBlock.Kind>(gridMapData.rockRecords[i], () => gridMapData.rockRecords.Length,(record) => record.kinds, UnmovableBlock.Kind.None, map, i );
                AddEntityFromRecord<HeavyBlock, HeavyBlockRecord, HeavyBlock.Kind>(gridMapData.heavyBlockRecords[i], () => gridMapData.heavyBlockRecords.Length, (record) => record.kinds, HeavyBlock.Kind.None, map, i );
                AddEntityFromRecord<FragileBlock, FragileBlockRecord, FragileBlock.Kind>(gridMapData.fragileBlockRecords[i], () => gridMapData.fragileBlockRecords.Length, (record) => record.kinds, FragileBlock.Kind.None, map, i );
                
            }

            return map;

            // for (int i = 0; i < map.GetLength(); i++)
            // {
            //     // Ground
            //     if (gridMapData.groundRecords != null)
            //     {
            //         foreach (var kind in gridMapData.groundRecords[i].kinds)
            //         {
            //             if (kind != Ground.Kind.None)
            //             {
            //                 map.AddEntity(i, new Ground(kind, map.ToVector(i)));
            //
            //             }
            //         }
            //     }
            //     else
            //     {
            //         Debug.LogWarning("groundRecords is not initialized properly!");
            //     }
            //     
            //     // BasicBlock
            //     if (gridMapData.basicBlockRecords != null)
            //     {
            //         foreach (var kind in gridMapData.basicBlockRecords[i].kinds)
            //         {
            //             if (kind != BasicBlock.Kind.None)
            //             {
            //                 map.AddEntity(i, new BasicBlock(kind, map.ToVector(i)));
            //
            //             }
            //         }
            //     }
            //     else
            //     {
            //         Debug.LogWarning("basicBlockRecords is not initialized properly!");
            //     }
            //
            //     // UnmovableBlock
            //     if (gridMapData.rockRecords != null)
            //     {
            //         foreach (var kind in gridMapData.rockRecords[i].kinds)
            //         {
            //             if (kind != UnmovableBlock.Kind.None)
            //             {
            //                 map.AddEntity(i, new UnmovableBlock(kind, map.ToVector(i)));
            //
            //             }
            //         }
            //     }
            //     else
            //     {
            //         Debug.LogWarning("rockRecords is not initialized properly!");
            //     }
            //     
            //     // HeavyBlock
            //     if (gridMapData.heavyBlockRecords != null)
            //     {
            //         if (gridMapData.heavyBlockRecords.Length == map.GetLength())
            //         {
            //             foreach (var kind in gridMapData.heavyBlockRecords[i].kinds)
            //             {
            //                 if (kind != HeavyBlock.Kind.None)
            //                 {
            //                     map.AddEntity(i, new HeavyBlock(kind, map.ToVector(i)));
            //
            //                 }
            //             }
            //         }
            //         else
            //         {
            //             Debug.LogError("heavyBlockRecords.Length is not equal to map.GetLength()!");
            //         }
            //     }
            //     else
            //     {
            //         Debug.LogWarning("heavyBlockRecords is not initialized properly!");
            //     }
            //     
            //     // FragileBlock
            //     if (gridMapData.fragileBlockRecords != null)
            //     {
            //         if (gridMapData.fragileBlockRecords.Length == map.GetLength())
            //         {
            //             foreach (var kind in gridMapData.fragileBlockRecords[i].kinds)
            //             {
            //                 if (kind != FragileBlock.Kind.None)
            //                 {
            //                     map.AddEntity(i, new FragileBlock(kind, map.ToVector(i)));
            //
            //                 }
            //             }
            //         }
            //         else
            //         {
            //             Debug.LogError("fragileBlockRecords.Length is not equal to map.GetLength()!");
            //         }
            //     }
            //     else
            //     {
            //         Debug.LogWarning("fragileBlockRecords is not initialized properly!");
            //     }
            // }

            // return map;
        }
        
        void AddEntityFromRecord<TEntity,TRecord,TKind>(TRecord record,Func<int> getRecordLength, Func<TRecord,TKind[]> funcKins,TKind noneValue, EntityGridMap map, int index) 
            where TEntity : IEntity
            where TRecord : class 
            where TKind : Enum
        {
            var kinds = funcKins(record);
            if (kinds != null)
            {
                if (getRecordLength() == map.GetLength())
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