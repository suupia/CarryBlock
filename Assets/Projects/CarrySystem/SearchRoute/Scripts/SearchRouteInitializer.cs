using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using VContainer;
#nullable enable


namespace Carry.CarrySystem.SearchRoute.Scripts
{
    public class SearchRouteInitializer : MonoBehaviour
    {
        IMapUpdater _entityGridMapSwitcher = null!;
        [Inject]
        public void Construct(
            IMapUpdater entityGridMapSwitcher)
        {
            _entityGridMapSwitcher = entityGridMapSwitcher;
        }

       async  void Start()
        {
            var runner = FindObjectOfType<NetworkRunner>();
            if (runner == null)
            {
                Debug.LogError($"NetworkRunner is not found.");
                return;
            }
            await UniTask.WaitUntil(() => runner.SceneManager.IsReady(runner));
            
            _entityGridMapSwitcher.InitUpdateMap(MapKey.Default,0);


            var startPos = new Vector2Int(2, 2);
            var endPos = new Vector2Int(10, 5);
            var orderInDirection = OrderInDirectionArrayContainer.CounterClockwiseStartingRightDirections;
            var map = _entityGridMapSwitcher.GetMap();
            Func<int, int, bool> isWall = (x, y) => map.GetSingleEntityList<IBlock>(new Vector2Int(x, y)).Count > 0;
            
            // var shortestRoute = _searchShortestRoute.NonDiagonalSearchShortestRoute( startPos,endPos,orderInDirection,isWall);
        }
    }

}
