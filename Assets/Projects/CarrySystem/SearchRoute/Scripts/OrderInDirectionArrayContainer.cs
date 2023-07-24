using UnityEngine;
using UnityEngine.Assertions;
#nullable enable

namespace Carry.CarrySystem.SearchRoute.Scripts
{
    public static class OrderInDirectionArrayContainer
    {
        // 6 4 2
        // 8 * 1
        // 7 5 3 の優先順位で判定していく
        public static readonly Vector2Int[] CounterClockwiseStartingRightDirections = new Vector2Int[]
        {
            Vector2Int.right, Vector2Int.right + Vector2Int.up, Vector2Int.right + Vector2Int.down, Vector2Int.up,
            Vector2Int.down, Vector2Int.left + Vector2Int.up, Vector2Int.left + Vector2Int.down, Vector2Int.left
        };

        // 7 5 3
        // 8 * 1
        // 6 4 2 の優先順位で判定していく
        public static readonly Vector2Int[] ClockwiseStartingRightDirections = new Vector2Int[]
        {
            Vector2Int.right, Vector2Int.right + Vector2Int.down, Vector2Int.right + Vector2Int.up, Vector2Int.down,
            Vector2Int.up, Vector2Int.left + Vector2Int.down, Vector2Int.left + Vector2Int.up, Vector2Int.left
        };

        // 8 6 4
        // 7 * 2
        // 5 3 1 の優先順位で判定していく
        public static readonly Vector2Int[] CounterClockwiseStartingRightDownDirections = new Vector2Int[]
        {
            Vector2Int.right + Vector2Int.down, Vector2Int.right, Vector2Int.down, Vector2Int.right + Vector2Int.up,
            Vector2Int.left + Vector2Int.down, Vector2Int.up, Vector2Int.left, Vector2Int.left + Vector2Int.up
        };

        // 8 7 5
        // 6 * 3
        // 4 2 1 の優先順位で判定していく
        public static readonly Vector2Int[] ClockwiseStartingRightDownDirections = new Vector2Int[]
        {
            Vector2Int.right + Vector2Int.down, Vector2Int.down, Vector2Int.right, Vector2Int.left + Vector2Int.down,
            Vector2Int.right + Vector2Int.up, Vector2Int.left, Vector2Int.up, Vector2Int.left + Vector2Int.up
        };

        // 1 3 5
        // 2 * 7
        // 4 6 8 の優先順位で判定していく
        public static readonly Vector2Int[] CounterClockwiseStartingLeftUpDirections = new Vector2Int[]
        {
            Vector2Int.left + Vector2Int.up, Vector2Int.left, Vector2Int.up, Vector2Int.left + Vector2Int.down,
            Vector2Int.right + Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.right + Vector2Int.down
        };

        // 1 2 4
        // 3 * 6
        // 5 7 8 の優先順位で判定していく
        public static readonly Vector2Int[] ClockwiseStartingLeftUpDirections = new Vector2Int[]
        {
            Vector2Int.left + Vector2Int.up, Vector2Int.up, Vector2Int.left, Vector2Int.right + Vector2Int.up,
            Vector2Int.left + Vector2Int.down, Vector2Int.right, Vector2Int.down, Vector2Int.right + Vector2Int.down
        };
        
        
        // 適当に追加
        // 4 2 1
        // 6 * 3
        // 8 7 5 の優先順位で判定していく
        public static readonly Vector2Int[] CounterClockwiseStartingRightUpDirections = new Vector2Int[]
        {
            Vector2Int.right + Vector2Int.up, Vector2Int.up, Vector2Int.right, Vector2Int.left + Vector2Int.up,
            Vector2Int.right + Vector2Int.down, Vector2Int.left, Vector2Int.down, Vector2Int.left + Vector2Int.down
        };

        /// <summary>
        /// 先頭と末尾の要素はそのままで、その他の要素を奇数と偶数で入れ替える
        /// </summary>
        /// <param name="originalArray"></param>
        /// <returns></returns>
        public static Vector2Int[] SwapPairwise(Vector2Int[] originalArray)
        {
            Assert.IsTrue(originalArray.Length % 2 == 0, "配列の要素数が偶数でなければなりません");
            Vector2Int[] swappedArray = new Vector2Int[originalArray.Length];
            originalArray.CopyTo(swappedArray, 0);

            for (int i = 1; i < swappedArray.Length - 1; i += 2)
            {
                (swappedArray[i], swappedArray[i + 1]) = (swappedArray[i + 1], swappedArray[i]); // 分解代入
            }


            return swappedArray;
        }
    }
}