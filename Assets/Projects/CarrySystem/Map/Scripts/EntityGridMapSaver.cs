using System.Collections;
using System.Collections.Generic;
using System.IO;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Spawners;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class EntityGridMapSaver
    {
        readonly string _folderPath;

        public EntityGridMapSaver()
        {
            _folderPath = Application.streamingAssetsPath + "/JsonFiles";
            Debug.Log($"セーブデータのファイルパスは　{_folderPath}");
        }
        // public class GridMapData
        // {
        //     public int width;
        //     public int height;
        //     public GroundRecord[] groundRecords;
        //     public RockRecord[] rockRecords;
        // }

        public void SaveMap(EntityGridMap map, int mapDataIndex)
        {
            EntityGridMapData entityGridMapData = new EntityGridMapData();
            var mapLength = map.GetLength();
            var rockRecords = new RockRecord[mapLength];
            var groundRecords = new GroundRecord[mapLength];


            for (int i = 0; i < mapLength; i++)
            {
                // //Rock
                // if (map.GetSingleEntity<Rock>(i) != null) rockRecords[i] = map.GetSingleEntity<Rock>(i).Kind;
                //
                // //Ground
                // if (map.GetSingleEntity<Ground>(i) != null) groundRecords[i] = map.GetSingleEntity<Ground>(i).Kind;


            }

            entityGridMapData.rockRecords = rockRecords;
            entityGridMapData.groundRecords = groundRecords;

            Save(entityGridMapData, mapDataIndex);
        }

         void Save(EntityGridMapData entityGridMapData, int mapDataIndex)
        {
            string json = JsonUtility.ToJson(entityGridMapData);
            string filePath = GetFilePath(mapDataIndex);
            using (StreamWriter
                   streamWriter = new StreamWriter(filePath)) //using構文によってDispose()（Close()と同じようなもの）が自動的に呼ばれる
            {
                streamWriter.Write(json);
                streamWriter.Flush();
            }
        }
        
         string GetFilePath(int index)
        {
            return _folderPath + $"/HexagonMapData{index}.json";

        }

        public bool IsExitFile(int index)
        {
            return File.Exists(GetFilePath(index));
        }
    }
}