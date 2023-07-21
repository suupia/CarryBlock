using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Spawners;
using Fusion;
using Projects.CarrySystem.Block.Scripts;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.RoutingAlgorithm.Scripts
{
    public class RoutePresenterBuilder
    {
        [Inject] NetworkRunner _runner;
        IEnumerable<RoutePresenter_Net> _routePresenters =  new List<RoutePresenter_Net>();
        
        [Inject]
        public RoutePresenterBuilder()
        {
        }
        
        public void Build(EntityGridMap map)
        {
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
            
            // TilePresenterをドメインのEntityGridMapに紐づける
            //   AttachTilePresenter(routePresenters, map);  // ToDo: searchShortestRouteを引数に渡す必要がある

            _routePresenters = routePresenters;
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

         void AttachTilePresenter(IEnumerable<RoutePresenter_Net> routePresenters , SearchShortestRoute searchShortestRoute)
        {
            for (int i = 0; i < routePresenters.Count(); i++)
            {
                var routePresenter = routePresenters.ElementAt(i);

                // RegisterTilePresenter()の前なのでSetEntityActiveData()を実行する必要がある
                // Presenterの初期化処理みたいなもの
                // var existGround = searchShortestRoute.GetSingleEntity<Ground>(i) != null;
                // var existRock = searchShortestRoute.GetSingleEntity<Rock>(i) != null;
                // var existBasicBlock = searchShortestRoute.GetSingleEntity<BasicBlock>(i) != null;
                
                // if(existRock) Debug.Log($"existGround: {existGround}, existRock: {existRock}, existBasicBlock: {existBasicBlock}");

                // routePresenter.SetInitAllEntityActiveData(searchShortestRoute.GetAllEntityList(i)  );
                //
                // // mapにTilePresenterを登録
                // searchShortestRoute.RegisterTilePresenter(routePresenter, i);
                
                // RoutePresenter用に書き直し
                routePresenter.SetActive(true);  // ToDo: 初期化の処理
                
                searchShortestRoute.RegisterRoutePresenter(routePresenter, i);
                
                
                
            }
        }
    }
}