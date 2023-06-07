using NUnit.Framework;
using UnityEngine;

namespace Nuts.Utility.Tests
{
    public class UtilityTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestReflectVectorYZero()
        {
            var vector1 = new Vector3(1, 0, 2);
            var axis1 = new Vector3(0, 0, 1);
            Assert.That(Scripts.NutsUtility.ReflectVectorYZero(vector1, axis1), Is.EqualTo(new Vector3(-1, 0, 2)));

            var vector2 = new Vector3(-1, 0, 2);
            var axis2 = new Vector3(0, 0, 1);
            Assert.That(Scripts.NutsUtility.ReflectVectorYZero(vector2, axis2), Is.EqualTo(new Vector3(1, 0, 2)));

            var vector3 = new Vector3(1, 2, 2);
            var axis3 = new Vector3(0, -4, 1);
            Assert.That(Scripts.NutsUtility.ReflectVectorYZero(vector1, axis1), Is.EqualTo(new Vector3(-1, 0, 2)));
        }

        [Test]
        public void TestGaussSeidel()
        {
            // 方程式
            // 3x + 2y + z = 10 
            // x + 4y + z = 12
            // 2x + 2y + 5z = 21

            // 解 
            // (x, y, z) = (1, 2, 3)

            var matrix = new double[,]
            {
                { 3, 2, 1 },
                { 1, 4, 1 },
                { 2, 2, 5 }
            };
            var b = new double[] { 10, 12, 21 };
            var eps = 1e-15;
            var result = Scripts.NutsUtility.GaussSeidel(matrix, b, 10000, eps);

            Debug.Log("Error:" + result.Error);
            Debug.Log("Count:" + result.Iterator);


            Assert.That(result.Solution[0], Is.EqualTo(1).Within(0.001f));
            Assert.That(result.Solution[1], Is.EqualTo(2).Within(0.001f));
            Assert.That(result.Solution[2], Is.EqualTo(3).Within(0.001f));
        }
    }
}