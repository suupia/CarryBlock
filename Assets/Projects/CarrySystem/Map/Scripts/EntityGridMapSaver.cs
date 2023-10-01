using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.CarriableBlock.Scripts;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Spawners;
using Projects.CarrySystem.ItemBlock.Scripts;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class EntityGridMapSaver
    {

        public EntityGridMapSaver()
        {
        }

        public void SaveMap(EntityGridMap map,MapKey key, int mapDataIndex)
        {
            var mapLength = map.Length;
            var groundRecords = new GroundRecord[mapLength];
            var rockRecords = new RockRecord[mapLength];
            var basicBlockRecords = new BasicBlockRecord[mapLength];
            var heavyBlockRecords = new HeavyBlockRecord[mapLength];
            var fragileBlockRecords = new FragileBlockRecord[mapLength];
            var cannonBlockRecords = new CannonBlockRecord[mapLength];
            var treasureCoinRecords = new TreasureCoinRecord[mapLength];
            
            for (int i = 0; i < mapLength; i++)
            {
                groundRecords[i] = new GroundRecord();
                rockRecords[i] = new RockRecord();
                basicBlockRecords[i] = new BasicBlockRecord();
                heavyBlockRecords[i] = new HeavyBlockRecord();
                fragileBlockRecords[i] = new FragileBlockRecord();
                cannonBlockRecords[i] = new CannonBlockRecord();
                treasureCoinRecords[i] = new TreasureCoinRecord();
            }

            for (int i = 0; i < mapLength; i++)
            {
                var grounds = map.GetSingleEntityList<Ground>(i);
                groundRecords[i].kinds = grounds.Select(x => x.KindValue).ToArray();
                
                var rocks = map.GetSingleEntityList<UnmovableBlock>(i);
                rockRecords[i].kinds = rocks.Select(x => x.KindValue).ToArray();
                
                var basicBlocks = map.GetSingleEntityList<BasicBlock>(i);
                basicBlockRecords[i].kinds = basicBlocks.Select(x => x.KindValue).ToArray();
                
                var heavyBlocks = map.GetSingleEntityList<HeavyBlock>(i);
                heavyBlockRecords[i].kinds = heavyBlocks.Select(x => x.KindValue).ToArray();
                
                var fragileBlocks = map.GetSingleEntityList<FragileBlock>(i);
                fragileBlockRecords[i].kinds = fragileBlocks.Select(x => x.KindValue).ToArray();
                
                var cannonBlocks = map.GetSingleEntityList<CannonBlock>(i);
                cannonBlockRecords[i].kinds = cannonBlocks.Select(x => x.KindValue).ToArray();
                
                var treasureCoinBlocks = map.GetSingleEntityList<TreasureCoin>(i);
                treasureCoinRecords[i].kinds = treasureCoinBlocks.Select(x => x.KindValue).ToArray();
            }

            // 保存するデータの作成
            EntityGridMapData entityGridMapData = new EntityGridMapData();
            entityGridMapData.width = map.Width;
            entityGridMapData.height = map.Height;
            entityGridMapData.rockRecords = rockRecords;
            entityGridMapData.groundRecords = groundRecords;
            entityGridMapData.basicBlockRecords = basicBlockRecords;
            entityGridMapData.heavyBlockRecords = heavyBlockRecords;
            entityGridMapData.fragileBlockRecords = fragileBlockRecords;
            entityGridMapData.cannonBlockRecords = cannonBlockRecords;
            entityGridMapData.treasureCoinRecords = treasureCoinRecords;

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