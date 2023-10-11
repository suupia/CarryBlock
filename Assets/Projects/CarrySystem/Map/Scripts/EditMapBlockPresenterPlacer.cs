using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Spawners;
using Fusion;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    public class EditMapBlockPresenterPlacer : IPresenterPlacer
    {
        [Inject] readonly NetworkRunner _runner;
        readonly EditMapBlockBuilder _carryBlockBuilder;
        IEnumerable<IEntityPresenter> _entityPresenters =  new List<IEntityPresenter>();
        
        [Inject]
        public EditMapBlockPresenterPlacer(EditMapBlockBuilder carryBlockBuilder)
        {
            _carryBlockBuilder = carryBlockBuilder;
        }

         
        public void Place(EntityGridMap map)
        {
            // 以前のTilePresenterを削除
            DestroyTilePresenter();
            
            var (blockControllers, blockPresenterNets) = _carryBlockBuilder.Build(ref map);
            
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
            _entityPresenters = new List<EntityPresenterNet>();
        }
        
         void AttachTilePresenter(IReadOnlyList<EntityPresenterNet> blockPresenterNets , EntityGridMap map)
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