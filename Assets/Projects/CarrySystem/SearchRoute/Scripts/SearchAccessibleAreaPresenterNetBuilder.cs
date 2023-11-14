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
        IReadOnlyList<RoutePresenterNet> _routePresenters =  new List<RoutePresenterNet>();
         bool _isRoutePresentersInitialized;
         
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

        IReadOnlyList<RoutePresenterNet> SetUpPresenter(IGridMap map)
        {
            if (!_isRoutePresentersInitialized) //最初にすべてスポーン
            {
                // Delete previous routePresenters
                foreach (var routePresenter in _routePresenters)
                {
                    _runner.Despawn(routePresenter.Object);
                }
                _routePresenters = new List<RoutePresenterNet>();

                // spawn new routePresenters
                var routePresenterSpawner = new RoutePresenterNetSpawner(_runner);
                var routePresenters = new List<RoutePresenterNet>();
                for (int i = 0; i < map.Length; i++)
                {
                    var girdPos = map.ToVector(i);
                    var worldPos = GridConverter.GridPositionToWorldPosition(girdPos);
                    var routePresenter = routePresenterSpawner.SpawnPrefab(worldPos, Quaternion.identity);
                    routePresenters.Add(routePresenter);
                }

                _routePresenters = routePresenters;
                _isRoutePresentersInitialized = true;
            }

            return _routePresenters;
        }
        
    }
}