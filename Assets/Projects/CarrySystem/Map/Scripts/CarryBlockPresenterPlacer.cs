using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Gimmick.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using Fusion;
using Projects.CarrySystem.Item.Interfaces;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    public class CarryBlockPresenterPlacer : IPresenterPlacer
    {
        [Inject] readonly NetworkRunner _runner;
        readonly CarryBlockBuilder _carryBlockBuilder;
        IEnumerable<EntityPresenterNet> _blockPresenters =  new List<EntityPresenterNet>();
        
        [Inject]
        public CarryBlockPresenterPlacer(CarryBlockBuilder carryBlockBuilder)
        {
            _carryBlockBuilder = carryBlockBuilder;
        }

        
        public void Place(EntityGridMap map)
        {
            // 以前のTilePresenterを削除
            DestroyTilePresenter();
            
            var (blockControllers, blockPresenterNets) = _carryBlockBuilder.Build(map);
            
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
            _blockPresenters = new List<EntityPresenterNet>();
        }
        
        void AttachTilePresenter(IReadOnlyList<EntityPresenterNet> blockPresenterNets , EntityGridMap map)
        {
            for (int i = 0; i < blockPresenterNets.Count(); i++)
            {
                var blockPresenterNet = blockPresenterNets.ElementAt(i);

                // IBlockMonoDelegateが入っているので、そこからIBlockとIItemを取得して渡す
                var monoDelegate = map.GetSingleEntity<IBlockMonoDelegate>(i);
                var blocks = map.GetSingleEntityList<IBlock>(i).Cast<IEntity>();
                var items = map.GetSingleEntityList<IItem>(i).Cast<IEntity>();
                var gimmicks = map.GetSingleEntityList<IGimmick>(i).Cast<IEntity>();
                var entityList = blocks.Concat(items).Concat(gimmicks).Distinct(); // Distinct()は重複を削除する
                blockPresenterNet.SetInitAllEntityActiveData(entityList);

                // mapにTilePresenterを登録
                map.RegisterTilePresenter(blockPresenterNet, i);
                
                
            }
        }
    }
}