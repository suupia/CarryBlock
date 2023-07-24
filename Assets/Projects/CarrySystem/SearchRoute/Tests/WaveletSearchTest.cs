using System;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.SearchRoute.Scripts;
using NUnit.Framework;
using UnityEngine;

namespace Projects.CarrySystem.RoutingAlgorithm.Tests
{
    public class WaveletSearchTest
    {
        [Test]
        public void SearchAccessibleArea1_7x7_A()
        {
            int width = 7;
            int height = 7;
            var expectedBoolArray = new bool[width * height];
            var allFalseArray = new bool[width * height];
            var resultBoolArray = new bool[width * height];
            var initValue = -1;
            var edgeValue = -8;
            var outOfRangeValue = -88;
            var map = new NumericGridMap(width,height,initValue,edgeValue,outOfRangeValue);
            var newSearchShortestRoute = new WaveletSearchExecutor(map);

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

            var startPos = new Vector2Int(0, 2);
            var endPos = new Vector2Int(6, 6);
            var walls = new List<(int, int)>() { (3, 1), (4, 1), (5, 1), (6, 1), (3, 0) };
            var wallsIncludeStart = new List<(int, int)>(walls);
            wallsIncludeStart.Add((startPos.x, startPos.y));
            var wallsIncludeEnd = new List<(int, int)>(walls);
            wallsIncludeEnd.Add((endPos.x, endPos.y));

            // walls
            resultBoolArray =
                newSearchShortestRoute.SearchAccessibleArea(startPos, endPos, (x, y) => walls.Contains((x, y)));
            Assert.AreEqual(expectedBoolArray, resultBoolArray);

            // wallsIncludeStart
            resultBoolArray =
                newSearchShortestRoute.SearchAccessibleArea(startPos, endPos,
                    (x, y) => wallsIncludeStart.Contains((x, y)));
            Assert.AreEqual(allFalseArray, resultBoolArray);

            // wallsIncludeEnd
            resultBoolArray =
                newSearchShortestRoute.SearchAccessibleArea(startPos, endPos,
                    (x, y) => wallsIncludeEnd.Contains((x, y)));
            Assert.AreEqual(allFalseArray, resultBoolArray);
        }

        [Test]
        public void SearchAccessibleArea1_7x7_B()
        {
            int width = 7;
            int height = 7;
            var expectedBoolArray = new bool[width * height];
            var allFalseArray = new bool[width * height];
            var resultBoolArray = new bool[width * height];
            var initValue = -1;
            var edgeValue = -8;
            var outOfRangeValue = -88;
            var map = new NumericGridMap(width,height,initValue,edgeValue,outOfRangeValue);
            var newSearchShortestRoute = new WaveletSearchExecutor(map);

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

            var startPos = new Vector2Int(1, 1);
            var endPos = new Vector2Int(4, 5);
            var walls = new List<(int, int)>() { (3, 0), (3, 1), (4, 1), (4, 2), (4, 3), (3, 4), (3, 5), (3, 6) };
            var wallsIncludeStart = new List<(int, int)>(walls);
            wallsIncludeStart.Add((startPos.x, startPos.y));
            var wallsIncludeEnd = new List<(int, int)>(walls);
            wallsIncludeEnd.Add((endPos.x, endPos.y));

            // walls
            resultBoolArray =
                newSearchShortestRoute.SearchAccessibleArea(startPos, endPos, (x, y) => walls.Contains((x, y)));
            Assert.AreEqual(expectedBoolArray, resultBoolArray);

            // wallsIncludeStart
            resultBoolArray =
                newSearchShortestRoute.SearchAccessibleArea(startPos, endPos,
                    (x, y) => wallsIncludeStart.Contains((x, y)));
            Assert.AreEqual(allFalseArray, resultBoolArray);

            // wallsIncludeEnd
            resultBoolArray =
                newSearchShortestRoute.SearchAccessibleArea(startPos, endPos,
                    (x, y) => wallsIncludeEnd.Contains((x, y)));
            Assert.AreEqual(allFalseArray, resultBoolArray);
        }

        [Test]
        public void SearchAccessibleArea1_10x8_A()
        {
            int width = 10;
            int height = 8;
            var expectedBoolArray = new bool[width * height];
            var allFalseArray = new bool[width * height];
            var resultBoolArray = new bool[width * height];
            var initValue = -1;
            var edgeValue = -8;
            var outOfRangeValue = -88;
            var map = new NumericGridMap(width,height,initValue,edgeValue,outOfRangeValue);
            var newSearchShortestRoute = new WaveletSearchExecutor(map);

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

            var startPos = new Vector2Int(1, 3);
            var endPos = new Vector2Int(7, 6);
            var walls = new List<(int, int)>()
                { (6, 0), (7, 0), (8, 3), (0, 5), (1, 5), (2, 5), (3, 5), (3, 6), (3, 7) };
            var wallsIncludeStart = new List<(int, int)>(walls);
            wallsIncludeStart.Add((startPos.x, startPos.y));
            var wallsIncludeEnd = new List<(int, int)>(walls);
            wallsIncludeEnd.Add((endPos.x, endPos.y));

            // walls
            resultBoolArray =
                newSearchShortestRoute.SearchAccessibleArea(startPos, endPos, (x, y) => walls.Contains((x, y)));
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
                newSearchShortestRoute.SearchAccessibleArea(startPos, endPos,
                    (x, y) => wallsIncludeStart.Contains((x, y)));
            Assert.AreEqual(allFalseArray, resultBoolArray);

            // wallsIncludeEnd
            resultBoolArray =
                newSearchShortestRoute.SearchAccessibleArea(startPos, endPos,
                    (x, y) => wallsIncludeEnd.Contains((x, y)));
            Assert.AreEqual(allFalseArray, resultBoolArray);
        }

        [Test]
        public void SearchAccessibleArea3_7x7_A()
        {
            int width = 7;
            int height = 7;
            var expectedBoolArray = new bool[width * height];
            var allFalseArray = new bool[width * height];
            var resultBoolArray = new bool[width * height];
            var initValue = -1;
            var edgeValue = -8;
            var outOfRangeValue = -88;
            var map = new NumericGridMap(width,height,initValue,edgeValue,outOfRangeValue);
            var newSearchShortestRoute = new WaveletSearchExecutor(map);

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

            var startPos = new Vector2Int(0, 2);
            var endPos = new Vector2Int(6, 6);
            var walls = new List<(int, int)>() { (3, 1), (4, 1), (5, 1), (6, 1), (3, 0) };
            var wallsIncludeStart = new List<(int, int)>(walls);
            wallsIncludeStart.Add((startPos.x, startPos.y));
            var wallsIncludeEnd = new List<(int, int)>(walls);
            wallsIncludeEnd.Add((endPos.x, endPos.y));

            // walls
            resultBoolArray =
                newSearchShortestRoute.SearchAccessibleArea(startPos, endPos,
                    (x, y) => walls.Contains((x, y)),SearcherSize.SizeThree);
            Assert.AreEqual(expectedBoolArray, resultBoolArray);

            // wallsIncludeStart
            resultBoolArray = newSearchShortestRoute.SearchAccessibleArea(startPos, endPos,
                (x, y) => wallsIncludeStart.Contains((x, y)),SearcherSize.SizeThree);
            Assert.AreEqual(allFalseArray, resultBoolArray);

            // wallsIncludeEnd
            resultBoolArray =
                newSearchShortestRoute.SearchAccessibleArea(startPos, endPos,
                    (x, y) => wallsIncludeEnd.Contains((x, y)),SearcherSize.SizeThree);
            Assert.AreEqual(allFalseArray, resultBoolArray);
        }

        [Test]
        public void SearchAccessibleArea3_7x7_B()
        {
            int width = 7;
            int height = 7;
            var expectedBoolArray = new bool[width * height];
            var allFalseArray = new bool[width * height];
            var resultBoolArray = new bool[width * height];
            var initValue = -1;
            var edgeValue = -8;
            var outOfRangeValue = -88;
            var map = new NumericGridMap(width,height,initValue,edgeValue,outOfRangeValue);
            var newSearchShortestRoute = new WaveletSearchExecutor(map);

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

            var startPos = new Vector2Int(1, 1);
            var endPos = new Vector2Int(4, 5);
            var walls = new List<(int, int)>() { (3, 0), (3, 1), (4, 1), (4, 2), (4, 3), (3, 4), (3, 5), (3, 6) };
            var wallsIncludeStart = new List<(int, int)>(walls);
            wallsIncludeStart.Add((startPos.x, startPos.y));
            var wallsIncludeEnd = new List<(int, int)>(walls);
            wallsIncludeEnd.Add((endPos.x, endPos.y));

            // walls
            resultBoolArray =
                newSearchShortestRoute.SearchAccessibleArea(startPos, endPos,
                    (x, y) => walls.Contains((x, y)),SearcherSize.SizeThree);
            Assert.AreEqual(expectedBoolArray, resultBoolArray);

            // wallsIncludeStart
            resultBoolArray =
                newSearchShortestRoute.SearchAccessibleArea(startPos, endPos,
                    (x, y) => wallsIncludeStart.Contains((x, y)),SearcherSize.SizeThree);
            Assert.AreEqual(allFalseArray, resultBoolArray);

            // wallsIncludeEnd
            resultBoolArray =
                newSearchShortestRoute.SearchAccessibleArea(startPos, endPos,
                    (x, y) => wallsIncludeEnd.Contains((x, y)),SearcherSize.SizeThree);
            Assert.AreEqual(allFalseArray, resultBoolArray);
        }

        [Test]
        public void SearchAccessibleArea3_10x8_A()
        {
            int width = 10;
            int height = 8;
            var expectedBoolArray = new bool[width * height];
            var allFalseArray = new bool[width * height];
            var resultBoolArray = new bool[width * height];
            var initValue = -1;
            var edgeValue = -8;
            var outOfRangeValue = -88;
            var map = new NumericGridMap(width,height,initValue,edgeValue,outOfRangeValue);
            var newSearchShortestRoute = new WaveletSearchExecutor(map);

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

            var startPos = new Vector2Int(1, 3);
            var endPos = new Vector2Int(7, 6);
            var walls = new List<(int, int)>()
                { (6, 0), (7, 0), (8, 3), (0, 5), (1, 5), (2, 5), (3, 5), (3, 6), (3, 7) };
            var wallsIncludeStart = new List<(int, int)>(walls);
            wallsIncludeStart.Add((startPos.x, startPos.y));
            var wallsIncludeEnd = new List<(int, int)>(walls);
            wallsIncludeEnd.Add((endPos.x, endPos.y));

            // walls
            resultBoolArray =
                newSearchShortestRoute.SearchAccessibleArea(startPos, endPos,
                    (x, y) => walls.Contains((x, y)),SearcherSize.SizeThree);
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
                newSearchShortestRoute.SearchAccessibleArea(startPos, endPos,
                    (x, y) => wallsIncludeStart.Contains((x, y)),SearcherSize.SizeThree);
            Assert.AreEqual(allFalseArray, resultBoolArray);

            // wallsIncludeEnd
            resultBoolArray =
                newSearchShortestRoute.SearchAccessibleArea(startPos, endPos,
                    (x, y) => wallsIncludeEnd.Contains((x, y)),SearcherSize.SizeThree);
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