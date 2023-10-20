using System;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.SearchRoute.Scripts;
using NUnit.Framework;
using UnityEngine;

namespace Carry.CarrySystem.RoutingAlgorithm.Tests
{
    public class WaveletSearchTest
    {
        [Test]
        public void SearchAccessibleArea1_7x7_A()
        {
            var mapData = new Map7X7A();
            var newSearchShortestRoute = new WaveletSearchExecutor(mapData.Map);
            var searchAccessibleAreaExecutor = new SearchAccessibleAreaExecutor(mapData.Map, newSearchShortestRoute);
            var expectedBoolArray = new bool[mapData.Map.Length];
            var allFalseArray = new bool[mapData.Map.Length];

            var expectedTrueIndexes = new List<int>();
            expectedTrueIndexes = ContinuousAdd(0, 2, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(7, 9, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(14, 20, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(21, 27, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(28, 34, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(35, 41, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(42, 48, expectedTrueIndexes);

            for (int i = 0; i < expectedBoolArray.Length; i++)
            {
                expectedBoolArray[i] = expectedTrueIndexes.Contains(i);
            }

            // walls
            var resultBoolArray =
                searchAccessibleAreaExecutor.SearchAccessibleArea(mapData.StartPos,
                    (x, y) => mapData.Walls.Contains((x, y)));
            Assert.AreEqual(expectedBoolArray, resultBoolArray);

            // wallsIncludeStart
            resultBoolArray =
                searchAccessibleAreaExecutor.SearchAccessibleArea(mapData.StartPos,
                    (x, y) => mapData.WallsIncludeStart.Contains((x, y)));
            Assert.AreEqual(allFalseArray, resultBoolArray);

        }

        [Test]
        public void SearchAccessibleArea1_7x7_B()
        {
            var mapData = new Map7X7B();
            var newSearchShortestRoute = new WaveletSearchExecutor(mapData.Map);
            var searchAccessibleAreaExecutor = new SearchAccessibleAreaExecutor(mapData.Map, newSearchShortestRoute);
            var expectedBoolArray = new bool[mapData.Map.Length];
            var allFalseArray = new bool[mapData.Map.Length];

            var expectedTrueIndexes = new List<int>();
            expectedTrueIndexes = ContinuousAdd(0, 2, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(7, 9, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(14, 17, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(21, 24, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(28, 30, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(35, 37, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(42, 44, expectedTrueIndexes);

            for (int i = 0; i < expectedBoolArray.Length; i++)
            {
                expectedBoolArray[i] = expectedTrueIndexes.Contains(i);
            }

            // walls
            var resultBoolArray =
                searchAccessibleAreaExecutor.SearchAccessibleArea(mapData.StartPos,
                    (x, y) => mapData.Walls.Contains((x, y)));
            Assert.AreEqual(expectedBoolArray, resultBoolArray);

            // wallsIncludeStart
            resultBoolArray =
                searchAccessibleAreaExecutor.SearchAccessibleArea(mapData.StartPos,
                    (x, y) => mapData.WallsIncludeStart.Contains((x, y)));
            Assert.AreEqual(allFalseArray, resultBoolArray);
            
        }

        [Test]
        public void SearchAccessibleArea1_10x8_A()
        {
            var mapData = new Map10X8A();
            var newSearchShortestRoute = new WaveletSearchExecutor(mapData.Map);
            var searchAccessibleAreaExecutor = new SearchAccessibleAreaExecutor(mapData.Map, newSearchShortestRoute);
            var expectedBoolArray = new bool[mapData.Map.Length];
            var allFalseArray = new bool[mapData.Map.Length];

            var expectedTrueIndexes = new List<int>();
            expectedTrueIndexes = ContinuousAdd(0, 5, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(8, 9, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(10, 19, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(20, 29, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(30, 37, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(39, 39, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(40, 49, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(54, 59, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(64, 69, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(74, 79, expectedTrueIndexes);

            for (int i = 0; i < expectedBoolArray.Length; i++)
            {
                expectedBoolArray[i] = expectedTrueIndexes.Contains(i);
            }

            // walls
            var resultBoolArray =
                searchAccessibleAreaExecutor.SearchAccessibleArea(mapData.StartPos,
                    (x, y) => mapData.Walls.Contains((x, y)));
            for (int i = 0; i < resultBoolArray.Length; i++)
            {
                if (expectedBoolArray[i] != resultBoolArray[i])
                {
                    Debug.Log(
                        $"i:{i} expectedBoolArray[i]:{expectedBoolArray[i]} resultBoolArray[i]:{resultBoolArray[i]}");
                }
            }

            Assert.AreEqual(expectedBoolArray, resultBoolArray);

            // wallsIncludeStart
            resultBoolArray =
                searchAccessibleAreaExecutor.SearchAccessibleArea(mapData.StartPos,
                    (x, y) => mapData.WallsIncludeStart.Contains((x, y)));
            Assert.AreEqual(allFalseArray, resultBoolArray);
        }

        [Test]
        public void SearchAccessibleArea3_7x7_A()
        {
            var mapData = new Map7X7A();
            var newSearchShortestRoute = new WaveletSearchExecutor(mapData.Map);
            var searchAccessibleAreaExecutor = new SearchAccessibleAreaExecutor(mapData.Map, newSearchShortestRoute);
            var expectedBoolArray = new bool[mapData.Map.Length];
            var allFalseArray = new bool[mapData.Map.Length];

            var expectedTrueIndexes = new List<int>();
            expectedTrueIndexes = ContinuousAdd(0, 2, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(7, 9, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(14, 20, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(21, 27, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(28, 34, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(35, 41, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(42, 48, expectedTrueIndexes);

            for (int i = 0; i < expectedBoolArray.Length; i++)
            {
                expectedBoolArray[i] = expectedTrueIndexes.Contains(i);
            }

            // walls
            var resultBoolArray =
                searchAccessibleAreaExecutor.SearchAccessibleArea(mapData.StartPos,
                    (x, y) => mapData.Walls.Contains((x, y)), SearcherSize.SizeThree);
            Assert.AreEqual(expectedBoolArray, resultBoolArray);

            // wallsIncludeStart
            resultBoolArray = searchAccessibleAreaExecutor.SearchAccessibleArea(mapData.StartPos,
                (x, y) => mapData.WallsIncludeStart.Contains((x, y)), SearcherSize.SizeThree);
            Assert.AreEqual(allFalseArray, resultBoolArray);

        }

        [Test]
        public void SearchAccessibleArea3_7x7_B()
        {
            var mapData = new Map7X7B();
            var newSearchShortestRoute = new WaveletSearchExecutor(mapData.Map);
            var searchAccessibleAreaExecutor = new SearchAccessibleAreaExecutor(mapData.Map, newSearchShortestRoute);
            var expectedBoolArray = new bool[mapData.Map.Length];
            var allFalseArray = new bool[mapData.Map.Length];

            var expectedTrueIndexes = new List<int>();
            expectedTrueIndexes = ContinuousAdd(0, 2, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(7, 9, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(14, 16, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(21, 23, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(28, 30, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(35, 37, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(42, 44, expectedTrueIndexes);

            for (int i = 0; i < expectedBoolArray.Length; i++)
            {
                expectedBoolArray[i] = expectedTrueIndexes.Contains(i);
            }

            // walls
            var resultBoolArray =
                searchAccessibleAreaExecutor.SearchAccessibleArea(mapData.StartPos,
                    (x, y) => mapData.Walls.Contains((x, y)), SearcherSize.SizeThree);
            Assert.AreEqual(expectedBoolArray, resultBoolArray);

            // wallsIncludeStart
            resultBoolArray = searchAccessibleAreaExecutor.SearchAccessibleArea(mapData.StartPos,
                (x, y) => mapData.WallsIncludeStart.Contains((x, y)), SearcherSize.SizeThree);
            Assert.AreEqual(allFalseArray, resultBoolArray);

        }

        [Test]
        public void SearchAccessibleArea3_10x8_A()
        {
            var mapData = new Map10X8A();
            var newSearchShortestRoute = new WaveletSearchExecutor(mapData.Map);
            var searchAccessibleAreaExecutor = new SearchAccessibleAreaExecutor(mapData.Map, newSearchShortestRoute);
            var expectedBoolArray = new bool[mapData.Map.Length];
            var allFalseArray = new bool[mapData.Map.Length];

            var expectedTrueIndexes = new List<int>();
            expectedTrueIndexes = ContinuousAdd(0, 5, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(10, 17, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(20, 27, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(30, 37, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(40, 49, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(54, 59, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(64, 69, expectedTrueIndexes);
            expectedTrueIndexes = ContinuousAdd(74, 79, expectedTrueIndexes);

            for (int i = 0; i < expectedBoolArray.Length; i++)
            {
                expectedBoolArray[i] = expectedTrueIndexes.Contains(i);
            }

            // walls
            var resultBoolArray =
                searchAccessibleAreaExecutor.SearchAccessibleArea(mapData.StartPos,
                    (x, y) => mapData.Walls.Contains((x, y)), SearcherSize.SizeThree);
            for (int i = 0; i < resultBoolArray.Length; i++)
            {
                if (expectedBoolArray[i] != resultBoolArray[i])
                {
                    Debug.Log(
                        $"i:{i} expectedBoolArray[i]:{expectedBoolArray[i]} resultBoolArray[i]:{resultBoolArray[i]}");
                }
            }

            Assert.AreEqual(expectedBoolArray, resultBoolArray);

            // wallsIncludeStart
            resultBoolArray = searchAccessibleAreaExecutor.SearchAccessibleArea(mapData.StartPos,
                (x, y) => mapData.WallsIncludeStart.Contains((x, y)), SearcherSize.SizeThree);
            Assert.AreEqual(allFalseArray, resultBoolArray);
            
        }


        List<int> ContinuousAdd(int start, int end, List<int> list)
        {
            for (int i = start; i <= end; i++)
            {
                list.Add(i);
            }

            return list;
        }
    }
}