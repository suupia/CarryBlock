using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.SearchRoute.Scripts;
using NUnit.Framework;
using Projects.CarrySystem.SearchRoute.Scripts;
using UnityEngine;

namespace Projects.CarrySystem.RoutingAlgorithm.Tests
{
    public class SearchShortestRouteTest
    {
        [Test]
        public void SearchShortestRoute1_7x7_A()
        {
            var mapData = new Map7X7A();
            var waveletSearchExecutor = new WaveletSearchExecutor(mapData.Map);
            var searchShortestRouteExecutor = new SearchShortestRouteExecutor(waveletSearchExecutor);
            var expectedRouteList = new List<Vector2Int>()
            {
                new Vector2Int(0, 2),
                new Vector2Int(1, 3),
                new Vector2Int(2, 4),
                new Vector2Int(3,5),
                new Vector2Int(4,5),
                new Vector2Int(5, 5),
                new Vector2Int(6, 6),
            };

            var resultRouteList = searchShortestRouteExecutor.DiagonalSearchShortestRoute(mapData.StartPos, mapData.EndPos,
                OrderInDirectionArrayContainer.CounterClockwiseStartingRightDirections, (x, y) => mapData.Walls.Contains((x, y)));
            Assert.AreEqual(expectedRouteList, resultRouteList);
            
            resultRouteList = searchShortestRouteExecutor.DiagonalSearchShortestRoute(mapData.StartPos, mapData.EndPos,
                OrderInDirectionArrayContainer.CounterClockwiseStartingRightDirections, (x, y) => mapData.WallsIncludeStart.Contains((x, y)));
            Assert.AreEqual(new List<Vector2Int>(), resultRouteList);
            
            resultRouteList = searchShortestRouteExecutor.DiagonalSearchShortestRoute(mapData.StartPos, mapData.EndPos,
                OrderInDirectionArrayContainer.CounterClockwiseStartingRightDirections, (x, y) => mapData.WallsIncludeEnd.Contains((x, y)));
            Assert.AreEqual(new List<Vector2Int>(), resultRouteList);
        }
        
        [Test]
        public void SearchAccessibleArea1_7x7_B()
        {
            var mapData = new Map7X7B();
            var waveletSearchExecutor = new WaveletSearchExecutor(mapData.Map);
            var searchShortestRouteExecutor = new SearchShortestRouteExecutor(waveletSearchExecutor);
            var expectedRouteList = new List<Vector2Int>();
            
            var resultRouteList = searchShortestRouteExecutor.DiagonalSearchShortestRoute(mapData.StartPos, mapData.EndPos,
                OrderInDirectionArrayContainer.CounterClockwiseStartingRightDirections, (x, y) => mapData.Walls.Contains((x, y)));
            Assert.AreEqual(expectedRouteList, resultRouteList);
            
            resultRouteList = searchShortestRouteExecutor.DiagonalSearchShortestRoute(mapData.StartPos, mapData.EndPos,
                OrderInDirectionArrayContainer.CounterClockwiseStartingRightDirections, (x, y) => mapData.WallsIncludeStart.Contains((x, y)));
            Assert.AreEqual(new List<Vector2Int>(), resultRouteList);
            
            resultRouteList = searchShortestRouteExecutor.DiagonalSearchShortestRoute(mapData.StartPos, mapData.EndPos,
                OrderInDirectionArrayContainer.CounterClockwiseStartingRightDirections, (x, y) => mapData.WallsIncludeEnd.Contains((x, y)));
            Assert.AreEqual(new List<Vector2Int>(), resultRouteList);
        }
        
        [Test]
        public void SearchAccessibleArea1_7x7_C()
        {
            var mapData = new Map7X7C();
            var waveletSearchExecutor = new WaveletSearchExecutor(mapData.Map);
            var searchShortestRouteExecutor = new SearchShortestRouteExecutor(waveletSearchExecutor);
            var expectedRouteList = new List<Vector2Int>()
            {
                new Vector2Int(0, 5),
                new Vector2Int(1, 4),
                new Vector2Int(1, 3),
                new Vector2Int(2,3),
                new Vector2Int(3,3),
                new Vector2Int(3, 4),
                new Vector2Int(3, 5),
                new Vector2Int(4, 5),
                new Vector2Int(5, 5),
                new Vector2Int(6, 4),
                new Vector2Int(6, 3),
                new Vector2Int(6, 2),
                new Vector2Int(6, 1),
                new Vector2Int(6, 0),
            };

            var resultRouteList = searchShortestRouteExecutor.DiagonalSearchShortestRoute(mapData.StartPos, mapData.EndPos,
                OrderInDirectionArrayContainer.CounterClockwiseStartingRightDirections, (x, y) => mapData.Walls.Contains((x, y)));
            Assert.AreEqual(expectedRouteList, resultRouteList);

            resultRouteList = searchShortestRouteExecutor.DiagonalSearchShortestRoute(mapData.StartPos, mapData.EndPos,
                OrderInDirectionArrayContainer.CounterClockwiseStartingRightDirections, (x, y) => mapData.WallsIncludeStart.Contains((x, y)));
            Assert.AreEqual(new List<Vector2Int>(), resultRouteList);
            
            resultRouteList = searchShortestRouteExecutor.DiagonalSearchShortestRoute(mapData.StartPos, mapData.EndPos,
                OrderInDirectionArrayContainer.CounterClockwiseStartingRightDirections, (x, y) => mapData.WallsIncludeEnd.Contains((x, y)));
            Assert.AreEqual(new List<Vector2Int>(), resultRouteList);
            
        }

    }

}