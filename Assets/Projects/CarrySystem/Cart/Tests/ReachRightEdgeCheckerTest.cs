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
    public void CalcContinuousCenterEvenArrayLength()
    {
        var reachRightEdgeChecker = new ReachRightEdgeChecker();
        var testArray = new bool[]{false, false, false, false, true, true, true, true};

        var result = reachRightEdgeChecker.CalcContinuousCenter(testArray, 3);
        Assert.That(result, Is.EqualTo(5));
    }
    
    [Test]
    public void CalcContinuousCenterOddArrayLength()
    {
        var reachRightEdgeChecker = new ReachRightEdgeChecker();
        var testArray = new bool[]{true, true, false, true, true, true, true};

        var result = reachRightEdgeChecker.CalcContinuousCenter(testArray, 3);
        Assert.That(result, Is.EqualTo(4));
    }

    [Test]
    public void CalcContinuousCenterEmptyArray()
    {
        var reachRightEdgeChecker = new ReachRightEdgeChecker();
        var testArray = new bool[]{};

        var result = reachRightEdgeChecker.CalcContinuousCenter(testArray, 3);
        Assert.That(result, Is.EqualTo(-1));
    }
    
    [Test]
    public void CalcContinuousCenterEvenContinuous()
    {
        var reachRightEdgeChecker = new ReachRightEdgeChecker();
        var testArray = new bool[]{false, true, true, true, false, true, true, false};

        var result = reachRightEdgeChecker.CalcContinuousCenter(testArray, 2);
        Assert.That(result, Is.EqualTo(2));
    }
    
    [Test]
    public void CalcContinuousCenterContinuous0()
    {
        var reachRightEdgeChecker = new ReachRightEdgeChecker();
        var testArray = new bool[]{false, true, true, true, false, true, true, false};

        var result = reachRightEdgeChecker.CalcContinuousCenter(testArray, 0);
        Assert.That(result, Is.EqualTo(-1));
    }
    
    [Test]
    public void CalcContinuousCenterContinuousNumOverArrayLength()
    {
        var reachRightEdgeChecker = new ReachRightEdgeChecker();
        var testArray = new bool[]{false, true, true, true, false, true, true, false};

        var result = reachRightEdgeChecker.CalcContinuousCenter(testArray, 100);
        Assert.That(result, Is.EqualTo(-1));
    }
    
    [Test]
    public void CalcContinuousCenterAllTrue()
    {
        var reachRightEdgeChecker = new ReachRightEdgeChecker();
        var testArray = new bool[]{true, true, true, true, true, true, true, true};

        var result = reachRightEdgeChecker.CalcContinuousCenter(testArray, 3);
        Assert.That(result, Is.EqualTo(3));
    }
}
