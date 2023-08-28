using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
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
                // Ground
                if (gridMapData.groundRecords != null)
                {
                    foreach (var kind in gridMapData.groundRecords[i].kinds)
                    {
                        if (kind != Ground.Kind.None)
                        {
                            map.AddEntity(i, new Ground(kind, map.ToVector(i)));

                        }
                    }
                }
                else
                {
                    Debug.LogWarning("groundRecords is not initialized properly!");
                }
                
                // BasicBlock
                if (gridMapData.basicBlockRecords != null)
                {
                    foreach (var kind in gridMapData.basicBlockRecords[i].kinds)
                    {
                        if (kind != BasicBlock.Kind.None)
                        {
                            map.AddEntity(i, new BasicBlock(kind, map.ToVector(i)));

                        }
                    }
                }
                else
                {
                    Debug.LogWarning("basicBlockRecords is not initialized properly!");
                }

                // UnmovableBlock
                if (gridMapData.rockRecords != null)
                {
                    foreach (var kind in gridMapData.rockRecords[i].kinds)
                    {
                        if (kind != UnmovableBlock.Kind.None)
                        {
                            map.AddEntity(i, new UnmovableBlock(kind, map.ToVector(i)));

                        }
                    }
                }
                else
                {
                    Debug.LogWarning("rockRecords is not initialized properly!");
                }
                
                // HeavyBlock
                if (gridMapData.heavyBlockRecords != null)
                {
                    if (gridMapData.heavyBlockRecords.Length == map.GetLength())
                    {
                        foreach (var kind in gridMapData.heavyBlockRecords[i].kinds)
                        {
                            if (kind != HeavyBlock.Kind.None)
                            {
                                map.AddEntity(i, new HeavyBlock(kind, map.ToVector(i)));

                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("heavyBlockRecords.Length is not equal to map.GetLength()!");
                    }
                }
                else
                {
                    Debug.LogWarning("heavyBlockRecords is not initialized properly!");
                }
                
                // FragileBlock
                if (gridMapData.fragileBlockRecords != null)
                {
                    if (gridMapData.fragileBlockRecords.Length == map.GetLength())
                    {
                        foreach (var kind in gridMapData.fragileBlockRecords[i].kinds)
                        {
                            if (kind != FragileBlock.Kind.None)
                            {
                                map.AddEntity(i, new FragileBlock(kind, map.ToVector(i)));

                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("fragileBlockRecords.Length is not equal to map.GetLength()!");
                    }
                }
                else
                {
                    Debug.LogWarning("fragileBlockRecords is not initialized properly!");
                }
            }

            return map;
        }
        
        void ProcessRecord<TEntity,TKind>(TKind[]? records, EntityGridMap map, int index, TKind noneValue) 
            where TEntity : IBlock , new() 
            where TKind : Enum
        {
            if (records != null)
            {
                if (records.Length == map.GetLength())
                {
                    foreach (var kind in records[index].kinds)
                    {
                        if (!kind.Equals(noneValue))
                        {
                            // This will create a new entity
                            var entity = Generic.Construct<IBlock, TKind, Vector2Int>(kind, map.ToVector(index));
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