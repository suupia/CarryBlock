using System.Linq;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.CarriableBlock.Scripts;
using Carry.CarrySystem.Entity.Scripts;
using Projects.CarrySystem.Gimmick.Scripts;
using Projects.CarrySystem.Item.Scripts;

namespace Carry.CarrySystem.Map.Scripts
{
    public class EntityGridMapDataConverter
    {
        public EntityGridMapData Convert(EntityGridMap map)
        {
            var mapLength = map.Length;
            var groundRecords = new GroundRecord[mapLength];
            var rockRecords = new RockRecord[mapLength];
            var basicBlockRecords = new BasicBlockRecord[mapLength];
            var heavyBlockRecords = new HeavyBlockRecord[mapLength];
            var fragileBlockRecords = new FragileBlockRecord[mapLength];
            var confusionBlockRecords = new ConfusionBlockRecord[mapLength];
            var cannonBlockRecords = new CannonBlockRecord[mapLength];
            var treasureCoinRecords = new TreasureCoinRecord[mapLength];
            var spikeGimmickRecords = new SpikeGimmickRecord[mapLength];
            
            for (int i = 0; i < mapLength; i++)
            {
                groundRecords[i] = new GroundRecord();
                rockRecords[i] = new RockRecord();
                basicBlockRecords[i] = new BasicBlockRecord();
                heavyBlockRecords[i] = new HeavyBlockRecord();
                fragileBlockRecords[i] = new FragileBlockRecord();
                confusionBlockRecords[i] = new ConfusionBlockRecord();
                cannonBlockRecords[i] = new CannonBlockRecord();
                treasureCoinRecords[i] = new TreasureCoinRecord();
                spikeGimmickRecords[i] = new SpikeGimmickRecord();
            }

            for (int i = 0; i < mapLength; i++)
            {
                var grounds = map.GetSingleTypeList<Ground>(i);
                groundRecords[i].kinds = grounds.Select(x => x.KindValue).ToArray();
                
                var rocks = map.GetSingleTypeList<UnmovableBlock>(i);
                rockRecords[i].kinds = rocks.Select(x => x.KindValue).ToArray();
                
                var basicBlocks = map.GetSingleTypeList<BasicBlock>(i);
                basicBlockRecords[i].kinds = basicBlocks.Select(x => x.KindValue).ToArray();
                
                var heavyBlocks = map.GetSingleTypeList<HeavyBlock>(i);
                heavyBlockRecords[i].kinds = heavyBlocks.Select(x => x.KindValue).ToArray();
                
                var fragileBlocks = map.GetSingleTypeList<FragileBlock>(i);
                fragileBlockRecords[i].kinds = fragileBlocks.Select(x => x.KindValue).ToArray();
                
                var confusionBlocks = map.GetSingleTypeList<ConfusionBlock>(i);
                confusionBlockRecords[i].kinds = confusionBlocks.Select(x => x.KindValue).ToArray();
                
                var cannonBlocks = map.GetSingleTypeList<CannonBlock>(i);
                cannonBlockRecords[i].kinds = cannonBlocks.Select(x => x.KindValue).ToArray();
                
                var treasureCoinBlocks = map.GetSingleTypeList<TreasureCoin>(i);
                treasureCoinRecords[i].kinds = treasureCoinBlocks.Select(x => x.KindValue).ToArray();
                
                var spikes = map.GetSingleTypeList<SpikeGimmick>(i);
                spikeGimmickRecords[i].kinds = spikes.Select(x => x.KindValue).ToArray();
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
            entityGridMapData.confusionBlockRecords = confusionBlockRecords;
            entityGridMapData.cannonBlockRecords = cannonBlockRecords;
            entityGridMapData.treasureCoinRecords = treasureCoinRecords;
            entityGridMapData.spikeGimmickRecords = spikeGimmickRecords;

            return entityGridMapData;
        }
    }
}