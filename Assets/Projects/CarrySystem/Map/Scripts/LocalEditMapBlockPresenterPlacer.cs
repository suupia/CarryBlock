using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Spawners;
using Fusion;
using UniRx;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    public class LocalEditMapBlockPresenterPlacer : IPresenterPlacer
    {
        readonly LocalEditMapBlockBuilder _localEditMapBlockBuilder;
        IEnumerable<IEntityPresenter> _entityPresenters =  new List<IEntityPresenter>();
        
        [Inject]
        public LocalEditMapBlockPresenterPlacer(LocalEditMapBlockBuilder localEditMapBlockBuilder)
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
        
         void AttachTilePresenter(IReadOnlyList<EntityPresenterLocal> blockPresenterNets , EntityGridMap map)
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