using System.Collections.Generic;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.SearchRoute.Scripts;
using UnityEngine;
using UnityEngine.PlayerLoop;
using BitStream = Fusion.Protocol.BitStream;
#nullable enable
namespace Carry.CarrySystem.Cart.Scripts
{
    public class ReachRightEdgeChecker
    {
        /// <summary>
        /// if the cart cannot reach the right edge, return -1
        /// </summary>
        /// <param name="accessibleArea"></param>
        /// <param name="map"></param>
        /// <param name="searcherSize"></param>
        /// <returns></returns>
        public int CalcCartReachRightEdge(bool[] accessibleArea, SquareGridMap map, SearcherSize searcherSize)
        {
            bool[] rightEdgeArray = new bool[map.Height];
            for (int y = 0; y < map.Height; y++)
            {
                rightEdgeArray[y] = accessibleArea[map.ToSubscript(map.Width - 1, y)];
            }
            // Debug.Log($"rightEdgeArray:{string.Join("," , rightEdgeArray)}");

            // check if the rightEdgeArray has 3 consecutive true
            var consecutiveCount = (int)searcherSize;

            return CalcContinuousCenter(rightEdgeArray, consecutiveCount);
        }

        /// <summary>
        /// trueがcontinuousNumだけ連続しているところの中心のindexを返す
        /// 偶数の場合は左側のindexを返す
        /// </summary>
        /// <param name="array"></param>
        /// <param name="continuousNum"></param>
        /// <returns></returns>
        public int CalcContinuousCenter(bool[] array, int continuousNum)
        {
            var counter = 0;
            List<int> centerList = new List<int>();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i])
                {
                    counter++;
                    if (counter == continuousNum)
                    {
                        int continuousCenter = continuousNum % 2 == 1 ? i - (continuousNum -1 ) / 2 : i - continuousNum / 2;
                        centerList.Add(continuousCenter);
                        counter--;
                    }
                }
                else
                {
                    counter = 0;
                }
            }
            if (centerList.Count == 0)
            {
                return -1;
            }
            int centerListMiddle = centerList.Count % 2 == 1 ? ((centerList.Count)-1) / 2 : centerList.Count / 2 -1;
            return centerList[centerListMiddle];
        }
        
        public bool CanCartReachRightEdge(bool[] accessibleArea, SquareGridMap map, SearcherSize searcherSize)
        {
            var reachRightEdge = CalcCartReachRightEdge(accessibleArea, map, searcherSize);

            return ( 0 <= reachRightEdge &&  reachRightEdge < map.Height);
        }
    }
}