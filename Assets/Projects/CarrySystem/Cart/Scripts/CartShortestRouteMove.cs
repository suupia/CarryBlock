using System;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.SearchRoute.Scripts;
using JetBrains.Annotations;
using Projects.CarrySystem.Block.Interfaces;
using Projects.CarrySystem.SearchRoute.Scripts;
using UnityEngine;
using VContainer;

#nullable enable
namespace Carry.CarrySystem.Cart.Scripts
{
    public class CartShortestRouteMove
    {
        readonly ReachRightEdgeChecker _reachRightEdgeChecker;
        readonly WaveletSearchBuilder _waveletSearchBuilder;
        EntityGridMap? _map; // このクラスはMapを登録して使用する (コンストラクタでIMapUpdaterを受け取らない)
        
        [Inject]
        public CartShortestRouteMove(
            ReachRightEdgeChecker reachRightEdgeChecker
                )
        {
            _reachRightEdgeChecker = reachRightEdgeChecker; 

        }

        public void RegisterMap(EntityGridMap map)
        {
            _map = map;
        }
        
        public void MoveAlongWithShortestRoute()
        {
            if (_map == null)
            {
                Debug.LogError($"_map is null");
                return;
            }

            var searcherSize = SearcherSize.SizeThree;
            
            var waveletSearchExecutor = new WaveletSearchExecutor(_map); // RoutePresenterをかませる必要がないから直接new
            var _searchShortestRouteExecutor = new SearchShortestRouteExecutor(waveletSearchExecutor);
            var startPos = new Vector2Int(1, _map.Height % 2 == 1 ? (_map.Height - 1) / 2 : _map.Height / 2);
            var endPos = new Vector2Int(_map.Width - 2, _map.Height % 2 == 1 ? (_map.Height - 1) / 2 : _map.Height / 2); // ToDo: どこかで統一して宣言する
            Func<int, int, bool> isWall = (x, y) => _map.GetSingleEntityList<IBlock>(new Vector2Int(x, y)).Count > 0;
            var accessibleArea = waveletSearchExecutor.SearchAccessibleArea(startPos, endPos, isWall, searcherSize);
            var endPosY = _reachRightEdgeChecker.CalcCartReachRightEdge(accessibleArea,_map, searcherSize);
            var routeEndPos = new Vector2Int(endPos.x, endPosY);

            var route = _searchShortestRouteExecutor.DiagonalSearchShortestRoute(startPos, routeEndPos, OrderInDirectionArrayContainer.CounterClockwiseStartingRightDirections,isWall,searcherSize);
            
            Debug.Log($"route : {string.Join(",",route)}");
        }
    }
}