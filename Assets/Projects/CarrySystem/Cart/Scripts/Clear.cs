using System;
using System.Threading;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.RoutingAlgorithm.Interfaces;
using Carry.CarrySystem.SearchRoute.Scripts;
using UnityEngine;

namespace Carry.CarrySystem.Cart.Scripts
{
    public class Clear
    {
        readonly IMapGetter _mapGetter;
        readonly WaveletSearchExecutor _waveletSearchExecutor;
        readonly SearchAccessibleAreaExecutor _searchAccessibleAreaExecutor;
        readonly SearchedMapExpander _searchedMapExpander;
        readonly IRoutePresenter?[] _routePresenters;
        readonly int _delayMilliSec = 30;
        CancellationTokenSource?[]? _ctss;
        long _beforeMaxValue = 0;
        
        readonly ReachRightEdgeChecker _reachRightEdgeChecker;



        public Clear(
            IMapGetter mapGetter,
            WaveletSearchExecutor waveletSearchExecutor, 
            SearchAccessibleAreaExecutor searchAccessibleAreaExecutor,
            SearchedMapExpander searchedMapExpander,
            ReachRightEdgeChecker reachRightEdgeChecker
            )
        {
            _mapGetter = mapGetter;
            _waveletSearchExecutor = waveletSearchExecutor;
            _searchAccessibleAreaExecutor = searchAccessibleAreaExecutor;
            _searchedMapExpander = searchedMapExpander;
            var mapLength = waveletSearchExecutor.Map.Length;
            _routePresenters = new IRoutePresenter[mapLength];
            _reachRightEdgeChecker = reachRightEdgeChecker;
        }


        public void Sample()
        {
            
            
        }

        bool IsClear()
        {
            var map = _mapGetter.GetMap();
            var startPos = new Vector2Int(1, map.Height / 2);
            Func<int, int, bool> isWall = (x, y) =>
                map.GetSingleEntity<IBlockMonoDelegate>(new Vector2Int(x, y))?.Blocks.Count > 0;
            var searcherSize = SearcherSize.SizeThree;

            var searchedMap = _waveletSearchExecutor.WaveletSearch(startPos, isWall, searcherSize);
            var accessibleAreaArray =_searchAccessibleAreaExecutor.SearchAccessibleArea(startPos, isWall, searcherSize);


            var result = _reachRightEdgeChecker.CanCartReachRightEdge(accessibleAreaArray, map, searcherSize);

            return result;

        }
    }
}