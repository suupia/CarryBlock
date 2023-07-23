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
        public void WaveletSearchTest_7x7()
        {
            int width = 7;
            int height = 7;
            var expectedBoolArray = new bool[width * height];
            var resultBoolArray = new bool[width * height];
            var newSearchShortestRoute = new NewSearchShortestRoute(width,height);

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
            var walls = new List<(int, int)>() { (3,1), (4,1), (5,1), (6,1), (3,0) };
            Func<int,int,bool> isWall = (x, y) => walls.Contains((x, y));

            var waveletResult = newSearchShortestRoute.WaveletSearch(startPos, endPos , isWall);

            for (int i = 0; i < resultBoolArray.Length; i++)
            {
                if (waveletResult[i] != newSearchShortestRoute.WallValue && waveletResult[i] != newSearchShortestRoute.InitValue)
                {
                    resultBoolArray[i] = true;
                }else
                {
                    resultBoolArray[i] = false;
                }
            }
            
            Assert.AreEqual(expectedBoolArray, resultBoolArray);
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