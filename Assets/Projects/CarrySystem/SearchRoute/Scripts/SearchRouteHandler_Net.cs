using System;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Fusion;
using Projects.CarrySystem.Block.Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Carry.CarrySystem.SearchRoute.Scripts
{
    /// <summary>
    /// テスト用にSearchShortestRouteを実行するクラス
    /// </summary>
    public class SearchRouteHandler_Net : NetworkBehaviour
    {
        [SerializeField] Vector2Int startPos;
        [SerializeField] Vector2Int endPos;
        Vector2Int[] orderInDirection = OrderInDirectionArrayContainer.CounterClockwiseStartingRightDirections;

        IMapUpdater _entityGridMapSwitcher;
        SearchShortestRoute _searchShortestRoute;
        
        [Inject]
        public void Construct(IMapUpdater entityGridMapSwitcher, SearchShortestRoute searchShortestRoute )
        {
            _entityGridMapSwitcher = entityGridMapSwitcher;
            _searchShortestRoute = searchShortestRoute;
        }


        void Update()
        {
            if(Runner == null) return;
            if(Runner.IsServer && Input.GetKeyDown(KeyCode.S))
            {
                Search();
            }
            
        }

        void Search()
        {
            
            var map = _entityGridMapSwitcher.GetMap();
            Func<int, int, bool> isWall = (x, y) => map.GetSingleEntityList<IBlock>(new Vector2Int(x, y)).Count > 0;
            
            var shortestRoute = _searchShortestRoute.NonDiagonalSearchShortestRoute( startPos,endPos,orderInDirection,isWall);

        }

    }
}