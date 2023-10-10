#nullable enable
using UnityEngine;

namespace Projects.CarrySystem.Item.Scripts
{
    public class TreasureCoinCounter
    {
        public int Count { get; private set; }

        public void Add(int count)
        {
            Count += count;
            Debug.Log($"Count: {Count}");
        }
    }
}