﻿using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Map.Interfaces;
using UnityEngine;
using VContainer;
#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class EditMapBlockPresenterPlacer : IPresenterPlacer
    {
        readonly EditMapBlockBuilder _editMapBlockBuilder;
        IEnumerable<IEntityPresenter> _entityPresenters =  new List<IEntityPresenter>();
        
        [Inject]
        public EditMapBlockPresenterPlacer(EditMapBlockBuilder editMapBlockBuilder)
        {
            _editMapBlockBuilder = editMapBlockBuilder;
        }

         
        public void Place(EntityGridMap map)
        {
            // 以前のTilePresenterを削除
            DestroyTilePresenter();
            
            var (blockControllers, entityPresenters) = _editMapBlockBuilder.Build(map);
            
            // BlockPresenterをドメインのEntityGridMapに紐づける
            AttachTilePresenter(entityPresenters, map);

            _entityPresenters = entityPresenters;
        }
        
        void DestroyTilePresenter()
        {
            Debug.Log($"DestroyTitlePresenter , _entityPresenters.Length = {_entityPresenters.Count()}");
            foreach (var entityPresenter in _entityPresenters)
            {
                entityPresenter.DestroyPresenter();
            }
            _entityPresenters = new List<IEntityPresenter>();
        }
        
         void AttachTilePresenter(IReadOnlyList<IEntityPresenter> entityPresenters , EntityGridMap map)
        {
            Debug.Log($"map.GetAllEntityList(0) joined = {string.Join("," ,map.GetAllEntityList(0))}");
            for (int i = 0; i < entityPresenters.Count(); i++)
            {
                var entityPresenter = entityPresenters.ElementAt(i);

                entityPresenter.SetInitAllEntityActiveData(map.GetAllEntityList(i));  // ここだけ、CarryBlockPresenterPlacerと違う

                // mapにTilePresenterを登録
                map.RegisterTilePresenter(entityPresenter, i);
                
                
            }
        }
        
    }
}