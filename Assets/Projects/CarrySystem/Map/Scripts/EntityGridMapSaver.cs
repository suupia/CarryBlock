using System.IO;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class EntityGridMapSaver
    {

        readonly EntityGridMapDataConverter _mapDataConverter;
        public EntityGridMapSaver(EntityGridMapDataConverter mapDataConverter)
        {
            _mapDataConverter = mapDataConverter;
        }

        public void SaveMap(EntityGridMap map,MapKey key, int mapDataIndex)
        {
            var entityGridMapData = _mapDataConverter.Convert(map);

            Save(entityGridMapData,key, mapDataIndex);
        }

         void Save(EntityGridMapData entityGridMapData, MapKey key,int mapDataIndex)
        {
            string json = JsonUtility.ToJson(entityGridMapData);
            string filePath =  EntityGridMapFileUtility.GetFilePath( key, mapDataIndex);
            using (StreamWriter
                   streamWriter = new StreamWriter(filePath)) //using構文によってDispose()（Close()と同じようなもの）が自動的に呼ばれる
            {
                streamWriter.Write(json);
                streamWriter.Flush();
            }
            Debug.Log($"Complete Save MapData:{key}_{mapDataIndex}\nfilePath:{filePath}");

        }
         
    }
}