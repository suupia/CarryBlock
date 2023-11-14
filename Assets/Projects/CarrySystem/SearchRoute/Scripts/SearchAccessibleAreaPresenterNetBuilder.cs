﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.RoutingAlgorithm.Interfaces;
using Fusion;
using UnityEngine;
using VContainer;
#nullable  enable

namespace Carry.CarrySystem.SearchRoute.Scripts
{
    /// <summary>
    /// ドメインスクリプトSearchAccessibleAreaExecutorにRoutePresenterを紐づけるクラス
    /// </summary>
    public class SearchAccessibleAreaPresenterNetBuilder : ISearchAccessibleAreaPresenterBuilder
    {
        [Inject] readonly NetworkRunner _runner = null!;
         
        [Inject]
        public SearchAccessibleAreaPresenterNetBuilder()
        {
            
        }
        
        public SearchAccessibleAreaPresenter BuildPresenter(IGridMap map)
        {
            var waveletSearchExecutor = new WaveletSearchExecutor(map);
            var searchedMapExpander = new SearchedMapExpander(waveletSearchExecutor);
            var searchAccessibleAreaExecutor = new SearchAccessibleAreaExecutor(waveletSearchExecutor,searchedMapExpander);
            
            var routePresenters = SetUpPresenter(map);
            var searchAccessibleAreaPresenter = new SearchAccessibleAreaPresenter(waveletSearchExecutor,searchAccessibleAreaExecutor,searchedMapExpander);
            searchAccessibleAreaPresenter.RegisterRoutePresenters(routePresenters);
            return searchAccessibleAreaPresenter;
            
        }

        IReadOnlyList<IRoutePresenter> SetUpPresenter(IGridMap map)
        {
            // foreachでの削除の処理が必要ないため、フィールドとして保持する必要がなく、さらに直接List<IRoutePresenter>に代入してよい
            
            // spawn new routePresenters
            var routePresenterSpawner = new RoutePresenterNetSpawner(_runner);
            var routePresenters = new List<IRoutePresenter>();
            for (int i = 0; i < map.Length; i++)
            {
                var girdPos = map.ToVector(i);
                var worldPos = GridConverter.GridPositionToWorldPosition(girdPos);
                var routePresenter = routePresenterSpawner.SpawnPrefab(worldPos, Quaternion.identity);
                routePresenters.Add(routePresenter);
            }

            return routePresenters;
        }
        
    }
}