using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using Fusion;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    public class CarryBlockPresenterPlacer : IBlockPresenterPlacer
    {
        [Inject] readonly NetworkRunner _runner;
        readonly BlockBuilder _blockBuilder;
        IEnumerable<BlockPresenterNet> _blockPresenters =  new List<BlockPresenterNet>();
        
        [Inject]
        public CarryBlockPresenterPlacer(BlockBuilder blockBuilder)
        {
            _blockBuilder = blockBuilder;
        }

        
        public void Place(EntityGridMap map)
        {
            // 以前のTilePresenterを削除
            DestroyTilePresenter();
            
            var (blockControllers, blockPresenterNets) = _blockBuilder.Build(ref map);
            
            // BlockPresenterをドメインのEntityGridMapに紐づける
            AttachTilePresenter(blockPresenterNets, map);

            _blockPresenters = blockPresenterNets;
        }
        
        void DestroyTilePresenter()
        {
            // マップの大きさが変わっても対応できるようにDestroyが必要
            // ToDo: マップの大きさを変えてテストをする 
            
            foreach (var blockPresenterNet in _blockPresenters)
            {
                _runner.Despawn(blockPresenterNet.Object);
            }
            _blockPresenters = new List<BlockPresenterNet>();
        }
        
        void AttachTilePresenter(IReadOnlyList<BlockPresenterNet> blockPresenterNets , EntityGridMap map)
        {
            for (int i = 0; i < blockPresenterNets.Count(); i++)
            {
                var blockPresenterNet = blockPresenterNets.ElementAt(i);

                // IBlockMonoDelegateが入っているので、そこからIBlockを取得して渡す
                blockPresenterNet.SetInitAllEntityActiveData(map.GetSingleEntity<IBlockMonoDelegate>(i).Blocks  );

                // mapにTilePresenterを登録
                map.RegisterTilePresenter(blockPresenterNet, i);
                
                
            }
        }
    }
}