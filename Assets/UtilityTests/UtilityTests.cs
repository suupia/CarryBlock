using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.TestTools;

public class UtilityTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void TestReflectVectorYZero()
    {
        Vector3 vector1 = new Vector3(1, 0, 2);
        Vector3 axis1 = new Vector3(0,0 , 1);
        Assert.That(Utility.ReflectVectorYZero(vector1,axis1),Is.EqualTo(new Vector3(-1,0,2)));

        Vector3 vector2 = new Vector3(-1, 0, 2);
        Vector3 axis2 = new Vector3(0, 0, 1);
        Assert.That(Utility.ReflectVectorYZero(vector2, axis2), Is.EqualTo(new Vector3(1, 0, 2)));

        Vector3 vector3 = new Vector3(1, 2, 2);
        Vector3 axis3 = new Vector3(0, -4, 1);
        Assert.That(Utility.ReflectVectorYZero(vector1, axis1), Is.EqualTo(new Vector3(-1, 0, 2)));
    }

    [Test]
    public void TestGaussSeidel()
    {
        // •û’öŽ®
        // 3x + 2y + z = 10 
        // x + 4y + z = 12
        // 2x + 2y + 5z = 21

        // ‰ð 
        // (x, y, z) = (1, 2, 3)

        var matrix = new double[,] {
            { 3, 2, 1 },
            { 1, 4, 1 },
            { 2, 2, 5 }
        };
        var b = new double[] { 10, 12, 21 };
        var eps = 1e-15;
        var result = Utility.GaussSeidel(matrix, b, 10000, eps);

        Debug.Log("Error:" + result.Error);
        Debug.Log("Count:" + result.Iterator);


        Assert.That(result.Solution[0], Is.EqualTo(1).Within(0.001f));
        Assert.That(result.Solution[1], Is.EqualTo(2).Within(0.001f));
        Assert.That(result.Solution[2], Is.EqualTo(3).Within(0.001f));
    }
}
