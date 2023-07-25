using System.Collections.Generic;
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
            int width = 7;
            int height = 7;
            var initValue = -1;
            var edgeValue = -8;
            var outOfRangeValue = -88;
            var map = new NumericGridMap(width, height, initValue, edgeValue, outOfRangeValue);
            var waveletSearchExecutor = new WaveletSearchExecutor(map);
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

            var startPos = new Vector2Int(0, 2);
            var endPos = new Vector2Int(6, 6);
            var walls = new List<(int, int)>() { (3, 1), (4, 1), (5, 1), (6, 1), (3, 0) };
            var wallsIncludeStart = new List<(int, int)>(walls);
            wallsIncludeStart.Add((startPos.x, startPos.y));
            var wallsIncludeEnd = new List<(int, int)>(walls);
            wallsIncludeEnd.Add((endPos.x, endPos.y));
            
            var resultRouteList = searchShortestRouteExecutor.DiagonalSearchShortestRoute(startPos, endPos,
                OrderInDirectionArrayContainer.CounterClockwiseStartingRightDirections, (x, y) => walls.Contains((x, y)));
            Assert.AreEqual(expectedRouteList, resultRouteList);
            
            resultRouteList = searchShortestRouteExecutor.DiagonalSearchShortestRoute(startPos, endPos,
                OrderInDirectionArrayContainer.CounterClockwiseStartingRightDirections, (x, y) => wallsIncludeStart.Contains((x, y)));
            Assert.AreEqual(new List<Vector2Int>(), resultRouteList);
            
            resultRouteList = searchShortestRouteExecutor.DiagonalSearchShortestRoute(startPos, endPos,
                OrderInDirectionArrayContainer.CounterClockwiseStartingRightDirections, (x, y) => wallsIncludeEnd.Contains((x, y)));
            Assert.AreEqual(new List<Vector2Int>(), resultRouteList);
        }
        
        [Test]
        public void SearchAccessibleArea1_7x7_B()
        {
            int width = 7;
            int height = 7;
            var initValue = -1;
            var edgeValue = -8;
            var outOfRangeValue = -88;
            var map = new NumericGridMap(width, height, initValue, edgeValue, outOfRangeValue);
            var waveletSearchExecutor = new WaveletSearchExecutor(map);
            var searchShortestRouteExecutor = new SearchShortestRouteExecutor(waveletSearchExecutor);
            var expectedRouteList = new List<Vector2Int>();


            var startPos = new Vector2Int(1, 1);
            var endPos = new Vector2Int(4, 5);
            var walls = new List<(int, int)>() { (3, 0), (3, 1), (4, 1), (4, 2), (4, 3), (3, 4), (3, 5), (3, 6) };
            var wallsIncludeStart = new List<(int, int)>(walls);
            wallsIncludeStart.Add((startPos.x, startPos.y));
            var wallsIncludeEnd = new List<(int, int)>(walls);
            wallsIncludeEnd.Add((endPos.x, endPos.y));

            var resultRouteList = searchShortestRouteExecutor.DiagonalSearchShortestRoute(startPos, endPos,
                OrderInDirectionArrayContainer.CounterClockwiseStartingRightDirections, (x, y) => walls.Contains((x, y)));
            Assert.AreEqual(expectedRouteList, resultRouteList);
            
            resultRouteList = searchShortestRouteExecutor.DiagonalSearchShortestRoute(startPos, endPos,
                OrderInDirectionArrayContainer.CounterClockwiseStartingRightDirections, (x, y) => wallsIncludeStart.Contains((x, y)));
            Assert.AreEqual(new List<Vector2Int>(), resultRouteList);
            
            resultRouteList = searchShortestRouteExecutor.DiagonalSearchShortestRoute(startPos, endPos,
                OrderInDirectionArrayContainer.CounterClockwiseStartingRightDirections, (x, y) => wallsIncludeEnd.Contains((x, y)));
            Assert.AreEqual(new List<Vector2Int>(), resultRouteList);
        }
        
        [Test]
        public void SearchAccessibleArea1_7x7_C()
        {
            int width = 7;
            int height = 7;
            var initValue = -1;
            var edgeValue = -8;
            var outOfRangeValue = -88;
            var map = new NumericGridMap(width, height, initValue, edgeValue, outOfRangeValue);
            var waveletSearchExecutor = new WaveletSearchExecutor(map);
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


            var startPos = new Vector2Int(0, 5);
            var endPos = new Vector2Int(6, 0);
            var walls = new List<(int, int)>() { (1, 1), (2, 4), (2, 5), (2, 6), (4, 0), (4, 1), (4, 2), (4, 3),(4,4) };
            var wallsIncludeStart = new List<(int, int)>(walls);
            wallsIncludeStart.Add((startPos.x, startPos.y));
            var wallsIncludeEnd = new List<(int, int)>(walls);
            wallsIncludeEnd.Add((endPos.x, endPos.y));

            var resultRouteList = searchShortestRouteExecutor.DiagonalSearchShortestRoute(startPos, endPos,
                OrderInDirectionArrayContainer.CounterClockwiseStartingRightDirections, (x, y) => walls.Contains((x, y)));
            Assert.AreEqual(expectedRouteList, resultRouteList);
            
            resultRouteList = searchShortestRouteExecutor.DiagonalSearchShortestRoute(startPos, endPos,
                OrderInDirectionArrayContainer.CounterClockwiseStartingRightDirections, (x, y) => wallsIncludeStart.Contains((x, y)));
            Assert.AreEqual(new List<Vector2Int>(), resultRouteList);
            
            resultRouteList = searchShortestRouteExecutor.DiagonalSearchShortestRoute(startPos, endPos,
                OrderInDirectionArrayContainer.CounterClockwiseStartingRightDirections, (x, y) => wallsIncludeEnd.Contains((x, y)));
            Assert.AreEqual(new List<Vector2Int>(), resultRouteList);
        }

    }

}