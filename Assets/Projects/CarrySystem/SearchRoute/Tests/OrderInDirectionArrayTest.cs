using Carry.CarrySystem.SearchRoute.Scripts;
using NUnit.Framework;
using UnityEngine;
using static Carry.CarrySystem.SearchRoute.Scripts.OrderInDirectionArrayContainer;

namespace Projects.CarrySystem.RoutingAlgorithm.Tests
{
    public class OrderInDirectionArrayTest
    {
        [Test]
        public void SwapPairwiseTest()
        {
            var vector = OrderInDirectionArrayContainer.SwapPairwise(CounterClockwiseStartingRightDirections);
            Debug.Log($"before {string.Join(",",CounterClockwiseStartingRightDirections )}");
            Debug.Log($"after {string.Join(",", vector)}");
            Debug.Log($"expected {string.Join(",", ClockwiseStartingRightDirections)}");
            Assert.AreEqual(ClockwiseStartingRightDirections, vector);
            
            vector = OrderInDirectionArrayContainer.SwapPairwise(ClockwiseStartingRightDirections);
            Assert.AreEqual(CounterClockwiseStartingRightDirections, vector);
            
            vector = OrderInDirectionArrayContainer.SwapPairwise(CounterClockwiseStartingRightDownDirections);
            Assert.AreEqual(ClockwiseStartingRightDownDirections, vector);
            
        }

    }
}