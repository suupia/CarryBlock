using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Scripts;
using Cysharp.Threading.Tasks;
using Fusion;
using Projects.CarrySystem.Block.Interfaces;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.SearchRoute.Scripts
{
    public class SearchRouteInitializer : MonoBehaviour
    {
        SearchShortestRoute _searchShortestRoute;
        SearchRouteUpdater _searchRouteUpdater;
        [Inject]
        public void Construct(
            SearchShortestRoute searchShortestRoute,
            SearchRouteUpdater searchRouteUpdater)
        {
            _searchShortestRoute = searchShortestRoute;
            _searchRouteUpdater = searchRouteUpdater;
        }

       async  void Start()
        {
            var runner = FindObjectOfType<NetworkRunner>();
            if(runner == null) Debug.LogError($"NetworkRunner is not found.");
            await UniTask.WaitUntil(() => runner.SceneManager.IsReady(runner));
            
            _searchRouteUpdater.InitUpdateMap(MapKey.Default, 1);


            var startPos = new Vector2Int(2, 2);
            var endPos = new Vector2Int(10, 5);
            var orderInDirection = OrderInDirectionArrayContainer.CounterClockwiseStartingRightDirections;
            var map = _searchRouteUpdater.GetMap();
            Func<int, int, bool> isWall = (x, y) => map.GetSingleEntityList<IBlock>(new Vector2Int(x, y)).Count > 0;
            
            var shortestRoute = _searchShortestRoute.NonDiagonalSearchShortestRoute( startPos,endPos,orderInDirection,isWall);

        }
    }

}
