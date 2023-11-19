using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.CarriableBlock.Scripts;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Spawners.Scripts;
using Projects.CarrySystem.Gimmick.Scripts;
using Projects.CarrySystem.Item.Scripts;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class EntityGridMapSaver
    {

        readonly EntityGridMapDataConverter _dataBuilder;
        public EntityGridMapSaver(EntityGridMapDataConverter dataBuilder)
        {
            _dataBuilder = dataBuilder;
        }

        public void SaveMap(EntityGridMap map,MapKey key, int mapDataIndex)
        {
            var entityGridMapData = _dataBuilder.Convert(map);

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