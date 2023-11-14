using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Spawners.Scripts;
using Carry.Utility;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class EntityGridMapLoader
    {
        readonly EntityGridMapBuilderLeaf _entityGridMapBuilderLeaf;
        
        public EntityGridMapLoader(EntityGridMapBuilderLeaf entityGridMapBuilderLeaf)
        {
            _entityGridMapBuilderLeaf = entityGridMapBuilderLeaf;
        }
        
        public EntityGridMap LoadEntityGridMap(MapKey key, int mapDataIndex)
        {
            var gridMapData = Load(key, mapDataIndex);
            return _entityGridMapBuilderLeaf.BuildEntityGridMap(gridMapData);
        }

        public EntityGridMap LoadDefaultEntityGridMap()
        {
            var defaultGridMapData = LoadDefault();
            return _entityGridMapBuilderLeaf.BuildEntityGridMap(defaultGridMapData);
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
    }
}