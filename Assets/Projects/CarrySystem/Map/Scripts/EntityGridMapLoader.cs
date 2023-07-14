using System.Collections;
using System.Collections.Generic;
using System.IO;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Spawners;
using UnityEngine;
using Carry.CarrySystem.Map.MapData;

#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class EntityGridMapLoader
    {
        public EntityGridMapLoader()
        {
        }

        public EntityGridMap LoadEntityGridMap(MapKey key, int mapDataIndex)
        {
            var gridMapData = Load(key, mapDataIndex);

            var map = new EntityGridMap(gridMapData.width, gridMapData.height);

            for (int i = 0; i < map.GetLength(); i++)
            {
                // Ground
                if (gridMapData.groundRecords[i].kind != Ground.Kind.None)
                {
                    map.AddEntity<Ground>(i, new Ground(gridMapData.groundRecords[i], map.GetVectorFromIndex(i)));
                }

                // Rock
                if (gridMapData.rockRecords[i].kind != Rock.Kind.None)
                {
                    map.AddEntity<Rock>(i, new Rock(gridMapData.rockRecords[i], map.GetVectorFromIndex(i)));
                }
            }

            return map;
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