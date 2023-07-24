﻿using System;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Spawners;
using Fusion;
using JetBrains.Annotations;
using Projects.CarrySystem.Block.Interfaces;
using Projects.CarrySystem.Block.Scripts;
using UnityEngine;
using VContainer;
#nullable  enable

namespace Carry.CarrySystem.SearchRoute.Scripts
{
    public class WaveletSearchBuilder
    {
        [Inject] NetworkRunner _runner;
        IEnumerable<RoutePresenter_Net> _routePresenters =  new List<RoutePresenter_Net>();

        [Inject]
        public WaveletSearchBuilder()
        {
        }

        public WaveletSearchExecutor Build(EntityGridMap map)
        {
            // ToDo: mapを受け取っているのはやりすぎ。もっと緩い制約でよいはず
            var routePresenterSpawner = new RoutePresenterSpawner(_runner);
            var routePresenters = new List<RoutePresenter_Net>();

            // 以前のTilePresenterを削除
            DestroyRoutePresenter();
            
            // RoutePresenterをスポーンさせる
            for (int i = 0; i < map.GetLength(); i++)
            {
                var girdPos = map.GetVectorFromIndex(i);
                var worldPos = GridConverter.GridPositionToWorldPosition(girdPos);
                var routePresenter = routePresenterSpawner.SpawnPrefab(worldPos, Quaternion.identity);
                routePresenters.Add(routePresenter);
            }
            
            _routePresenters = routePresenters;
            
           return  AttachRoutePresenter(routePresenters, map);

        }
        
        void DestroyRoutePresenter()
        {
            // マップの大きさが変わっても対応できるようにDestroyが必要
            // ToDo: マップの大きさを変えてテストをする 
            
            foreach (var routePresenter in _routePresenters)
            {
                _runner.Despawn(routePresenter.Object);
            }
            _routePresenters = new List<RoutePresenter_Net>();
        }

        WaveletSearchExecutor AttachRoutePresenter(IEnumerable<RoutePresenter_Net> routePresenters , EntityGridMap map)
        {
            for (int i = 0; i < routePresenters.Count(); i++)
            {
                var routePresenter = routePresenters.ElementAt(i);
                // RoutePresenter用に書き直し
                routePresenter.SetPresenterActive(false);  // ToDo: 初期化の処理

            }

            var waveletSearchExecutor = new WaveletSearchExecutor(map);
            waveletSearchExecutor.RegisterRoutePresenters( routePresenters);
            return waveletSearchExecutor;
        }
    }
}