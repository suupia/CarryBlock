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
            _folderPath = Application.streamingAssetsPath + "/JsonFiles/MapData";
            Debug.Log($"セーブデータのファイルパスは　{_folderPath}");
        }

        public void SaveMap(EntityGridMap map,MapKey key, int mapDataIndex)
        {
            var mapLength = map.GetLength();
            var rockRecords = new RockRecord[mapLength];
            var groundRecords = new GroundRecord[mapLength];

            for (int i = 0; i < mapLength; i++)
            {
                // Ground
                if (map.GetSingleEntity<Ground>(i) is {} ground) groundRecords[i] = ground.Record;
                
                // Rock
                if (map.GetSingleEntity<Rock>(i) is {} rock) rockRecords[i] = rock.Record;
                
            }

            // 保存するデータの作成
            EntityGridMapData entityGridMapData = new EntityGridMapData();
            entityGridMapData.width = map.Width;
            entityGridMapData.height = map.Height;
            entityGridMapData.rockRecords = rockRecords;
            entityGridMapData.groundRecords = groundRecords;

            Save(entityGridMapData,key, mapDataIndex);
        }

         void Save(EntityGridMapData entityGridMapData, MapKey key,int mapDataIndex)
        {
            string json = JsonUtility.ToJson(entityGridMapData);
            string filePath = GetFilePath(key, mapDataIndex);
            using (StreamWriter
                   streamWriter = new StreamWriter(filePath)) //using構文によってDispose()（Close()と同じようなもの）が自動的に呼ばれる
            {
                streamWriter.Write(json);
                streamWriter.Flush();
            }
        }
        
         string GetFilePath(MapKey key,  int index)
        {
            return _folderPath + $"/MapData_{key}_{index}.json";

        }

        public bool IsExitFile(MapKey key, int index)
        {
            return File.Exists(GetFilePath(key,index));
        }
    }
}