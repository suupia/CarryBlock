using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Projects.CarrySystem.SearchRoute.Scripts
{
    public static class SwapOrderInDirection
    {
        /// <summary>
        /// 先頭と末尾の要素はそのままで、その他の要素を奇数と偶数で入れ替える
        /// </summary>
        /// <param name="originalList"></param>
        /// <returns></returns>
        public static Vector2Int[] SwapPairwise(IReadOnlyList<Vector2Int> originalList)
        {
            Assert.IsTrue(originalList.Count % 2 == 0, "The number of elements in the array must be even");
            Vector2Int[] swappedArray = new Vector2Int[originalList.Count];

            for (int i = 0; i < originalList.Count; i++)
            {
                swappedArray[i] = originalList[i];
            }

            for (int i = 1; i < swappedArray.Length - 1; i += 2)
            {
                (swappedArray[i], swappedArray[i + 1]) = (swappedArray[i + 1], swappedArray[i]); // deconstructing assignment
            }

            return swappedArray;
        }

    }
}