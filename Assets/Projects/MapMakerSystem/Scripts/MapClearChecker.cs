#nullable enable

using System;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.SearchRoute.Scripts;
using UnityEngine;
using VContainer;

namespace Projects.MapMakerSystem.Scripts
{
    public class MapClearChecker: MonoBehaviour
    {
        SearchAccessibleAreaExecutor? _searchAccessibleAreaExecutor;
        ReachRightEdgeChecker _reachRightEdgeChecker = null!;
        MapValidator _mapValidator = null!;
        FloorTimerLocal _floorTimerLocal = null!;
        IMapGetter _mapGetter = null!;
                
        [Inject]
        public void Construct(
            ReachRightEdgeChecker reachRightEdgeChecker,
            FloorTimerLocal floorTimerLocal,
            IMapGetter mapGetter,
            MapValidator mapValidator)
        {
            _reachRightEdgeChecker = reachRightEdgeChecker;
            _floorTimerLocal = floorTimerLocal;
            _mapGetter = mapGetter;
            _mapValidator = mapValidator;
        }

        void Update()
        {
            if (!_floorTimerLocal.IsActive) return;

            var isClear = GetIsClear();

            if (isClear)
            {
                _mapValidator.StopTestPlay(true);
            }
        }

        bool GetIsClear()
        {
            var map = _mapGetter.GetMap();

            if (_searchAccessibleAreaExecutor == null)
            {
                var waveletSearchExecutor = new WaveletSearchExecutor(map);
                var searchedMapExpander = new SearchedMapExpander(waveletSearchExecutor);
                _searchAccessibleAreaExecutor =
                    new SearchAccessibleAreaExecutor(waveletSearchExecutor, searchedMapExpander);
            }

            var startPos = new Vector2Int(1, map.Height / 2);
            const SearcherSize searcherSize = SearcherSize.SizeThree;
            Func<int, int, bool> isWall = (x, y) => map.GetSingleEntityList<IBlock>(new Vector2Int(x, y))?.Count > 0;
            
            var accessibleArea = _searchAccessibleAreaExecutor.SearchAccessibleArea(startPos, isWall, searcherSize);

            return _reachRightEdgeChecker.CanCartReachRightEdge(accessibleArea, map, searcherSize);
        }
    }
}