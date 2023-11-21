using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Map.Interfaces;
using UnityEngine;
using VContainer;
#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class EditMapBlockPresenterPlacer : IPresenterPlacer
    {
        readonly PlaceablePresenterBuilder _placeablePresenterBuilder;
        IEnumerable<IPlaceablePresenter> _placeablePresenters =  new List<IPlaceablePresenter>();
        
        [Inject]
        public EditMapBlockPresenterPlacer(PlaceablePresenterBuilder placeablePresenterBuilder)
        {
            _placeablePresenterBuilder = placeablePresenterBuilder;
        }

         
        public void Place(EntityGridMap map)
        {
            // 以前のTilePresenterを削除
            DestroyTilePresenter();
            
            var (blockControllers, entityPresenters) = _placeablePresenterBuilder.Build(map);
            
            // BlockPresenterをドメインのEntityGridMapに紐づける
            AttachTilePresenter(entityPresenters, map);

            _placeablePresenters = entityPresenters;
        }
        
        void DestroyTilePresenter()
        {
            foreach (var entityPresenter in _placeablePresenters)
            {
                entityPresenter.DestroyPresenter();
            }
            _placeablePresenters = new List<IPlaceablePresenter>();
        }
        
         void AttachTilePresenter(IReadOnlyList<IPlaceablePresenter> entityPresenters , EntityGridMap map)
        {
            for (int i = 0; i < entityPresenters.Count(); i++)
            {
                var entityPresenter = entityPresenters.ElementAt(i);

                entityPresenter.SetInitAllEntityActiveData(map.GetAllEntityList(i));

                // mapにTilePresenterを登録
                map.RegisterTilePresenter(entityPresenter, i);
                
                
            }
        }
        
    }
}