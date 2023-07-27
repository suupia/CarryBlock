using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.SearchRoute.Scripts;
using NUnit.Framework;
using Projects.CarrySystem.RoutingAlgorithm.Tests;
using Projects.CarrySystem.SearchRoute.Scripts;
using UnityEngine;

public class ReachRightEdgeCheckerTest
{
    [Test]
    public void CalcContinuousCenterA()
    {
        var reachRightEdgeChecker = new ReachRightEdgeChecker();
        var testArray = new bool[]{false, false, false, false, true, true, true, true};

        var result = reachRightEdgeChecker.CalcContinuousCenter(testArray, 3);
        Assert.That(result, Is.EqualTo(5));
    }
    
    // 配列が偶数、配列が空、countinuousNumが偶数、countinuousNumが0、countinuousNumが配列の長さ以上、などのテストを追加
    // エラーが出ないようにいい感じに実装する
}
