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
        readonly string _folderPath;

        public  EntityGridMapLoader()
        {
            _folderPath = Application.streamingAssetsPath + "/JsonFiles";
            Debug.Log($"セーブデータのファイルパスは {_folderPath}");
        }

        public EntityGridMap LoadEntityGridMap(int mapDataIndex)
        {
            var gridMapData = Load(mapDataIndex);

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

        EntityGridMapData Load(int mapDataIndex)
        {
            EntityGridMapData entityGridMapData;

            string filePath = GetFilePath(mapDataIndex);

            if (IsExitFile(mapDataIndex))
            {
                using (StreamReader streamReader = new StreamReader(filePath))
                {
                    string data = streamReader.ReadToEnd();
                    streamReader.Close();
                    entityGridMapData = JsonUtility.FromJson<EntityGridMapData>(data);
                }
            }
            else
            {
                Debug.LogWarning($"パス:{filePath}にjsonファイルが存在しません");
                entityGridMapData = new DefaultEntityGridMapData();
            }

            return entityGridMapData;
        }

        string GetFilePath(int index)
        {
            return _folderPath + $"/HexagonMapData{index}.json";
        }

        bool IsExitFile(int index)
        {
            return File.Exists(GetFilePath(index));
        }
    }

}