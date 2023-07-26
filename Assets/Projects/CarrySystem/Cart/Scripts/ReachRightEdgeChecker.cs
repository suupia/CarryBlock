using Carry.CarrySystem.Map.Scripts;
using UnityEngine;
using UnityEngine.PlayerLoop;

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
            var counter = 0;
            for (int i = 0; i < rightEdgeArray.Length - 1; i++)
            {
                if (rightEdgeArray[i])
                {
                    counter++;
                    if (counter == consecutiveCount)
                    {
                        Debug.Log("Reachable");
                        return i;
                    }
                }
                else
                {
                    counter = 0;
                }
            }

            return -1;
        }
        
        public bool CanCartReachRightEdge(bool[] accessibleArea, SquareGridMap map, SearcherSize searcherSize)
        {
            var reachRightEdge = CalcCartReachRightEdge(accessibleArea, map, searcherSize);

            return ( 0 <= reachRightEdge &&  reachRightEdge < map.Height);
        }
    }
}