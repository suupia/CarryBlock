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
        IEnumerable<IEntityPresenter> _entityPresenters =  new List<IEntityPresenter>();
        
        [Inject]
        public CarryBlockPresenterPlacer(CarryBlockBuilder carryBlockBuilder)
        {
            _carryBlockBuilder = carryBlockBuilder;
        }

        
        public void Place(EntityGridMap map)
        {
            // 以前のTilePresenterを削除
            DestroyTilePresenter();
            
            var (blockControllers, entityPresenters) = _carryBlockBuilder.Build(map);
            
            // BlockPresenterをドメインのEntityGridMapに紐づける
            AttachTilePresenter(entityPresenters, map);

            _entityPresenters = entityPresenters;
        }
        
        void DestroyTilePresenter()
        {
            // マップの大きさが変わっても対応できるようにDestroyが必要
            // ToDo: マップの大きさを変えてテストをする 
            
            foreach (var entityPresenter in _entityPresenters)
            {
                entityPresenter.DestroyPresenter();
            }
            _entityPresenters = new List<IEntityPresenter>();
        }
        
        void AttachTilePresenter(IReadOnlyList<IEntityPresenter> entityPresenters , EntityGridMap map)
        {
            for (int i = 0; i < entityPresenters.Count(); i++)
            {
                var entityPresenter = entityPresenters.ElementAt(i);

                // IBlockMonoDelegateが入っているので、そこからIBlockとIItemを取得して渡す
                var blocks = map.GetSingleEntityList<IBlock>(i).Cast<IEntity>();
                var items = map.GetSingleEntityList<IItem>(i).Cast<IEntity>();
                var gimmicks = map.GetSingleEntityList<IGimmick>(i).Cast<IEntity>();
                var entityList = blocks.Concat(items).Concat(gimmicks).Distinct(); // Distinct()は重複を削除する
                entityPresenter.SetInitAllEntityActiveData(entityList);

                // mapにTilePresenterを登録
                map.RegisterTilePresenter(entityPresenter, i);
                
                
            }
        }
    }
}