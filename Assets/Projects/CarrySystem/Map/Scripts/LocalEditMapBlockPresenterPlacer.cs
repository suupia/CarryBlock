using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Map.Interfaces;
using VContainer;
#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class LocalEditMapBlockPresenterPlacer : IPresenterPlacer
    {
        readonly EditMapBlockBuilder _localEditMapBlockBuilder;
        IEnumerable<IEntityPresenter> _entityPresenters =  new List<IEntityPresenter>();
        
        [Inject]
        public LocalEditMapBlockPresenterPlacer(EditMapBlockBuilder localEditMapBlockBuilder)
        {
            _localEditMapBlockBuilder = localEditMapBlockBuilder;
        }

         
        public void Place(EntityGridMap map)
        {
            // 以前のTilePresenterを削除
            DestroyTilePresenter();
            
            var (blockControllers, blockPresenterNets) = _localEditMapBlockBuilder.Build(ref map);
            
            // BlockPresenterをドメインのEntityGridMapに紐づける
            AttachTilePresenter(blockPresenterNets, map);

            _entityPresenters = blockPresenterNets;
        }
        
        void DestroyTilePresenter()
        {
            foreach (var entityPresenter in _entityPresenters)
            {
                entityPresenter.DestroyPresenter();
            }
            _entityPresenters = new List<EntityPresenterLocal>();
        }
        
         void AttachTilePresenter(IReadOnlyList<IEntityPresenter> blockPresenterNets , EntityGridMap map)
        {
            for (int i = 0; i < blockPresenterNets.Count(); i++)
            {
                var blockPresenterNet = blockPresenterNets.ElementAt(i);

                blockPresenterNet.SetInitAllEntityActiveData(map.GetAllEntityList(i));  // ここだけ、CarryBlockPresenterPlacerと違う

                // mapにTilePresenterを登録
                map.RegisterTilePresenter(blockPresenterNet, i);
                
                
            }
        }
        
    }
}