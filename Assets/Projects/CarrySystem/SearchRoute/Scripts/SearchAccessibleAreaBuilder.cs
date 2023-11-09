using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Spawners.Scripts;
using Fusion;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;
#nullable  enable

namespace Carry.CarrySystem.SearchRoute.Scripts
{
    /// <summary>
    /// ドメインスクリプトSearchAccessibleAreaExecutorにRoutePresenterを紐づけるクラス
    /// </summary>
    public class SearchAccessibleAreaBuilder
    {
        [Inject] readonly NetworkRunner _runner = null!;
        IEnumerable<RoutePresenterNet> _routePresenters =  new List<RoutePresenterNet>();
         bool _isRoutePresentersInitialized;
         
        [Inject]
        public SearchAccessibleAreaBuilder()
        {
            
        }

        public SearchAccessibleAreaExecutor Build(SquareGridMap map)
        {
            var routePresenterSpawner = new RoutePresenterSpawner(_runner);
            var routePresenters = new List<RoutePresenterNet>();
            
            if (!_isRoutePresentersInitialized) //最初にすべてスポーン
            {
                // 以前のTilePresenterを削除
                DestroyRoutePresenter();

                // RoutePresenterをスポーンさせる
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
            else
            {
                routePresenters = _routePresenters.ToList();
            }

            var waveletSearchExecutor = new WaveletSearchExecutor(map);
            var searchAccessibleAreaExecutor = new SearchAccessibleAreaExecutor(map, waveletSearchExecutor);
            searchAccessibleAreaExecutor.RegisterRoutePresenters(routePresenters);
            return searchAccessibleAreaExecutor;
            
        }
        
        void DestroyRoutePresenter()
        {
            // マップの大きさが変わっても対応できるようにDestroyが必要
            // ToDo: マップの大きさを変えてテストをする 
            
            foreach (var routePresenter in _routePresenters)
            {
                _runner.Despawn(routePresenter.Object);
            }
            _routePresenters = new List<RoutePresenterNet>();
        }

    }
}