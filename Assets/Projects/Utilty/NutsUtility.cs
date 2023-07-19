#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Projects.Utility.Scripts
{
    public class NutsUtility
    {
        public static Vector3 SetYToZero(Vector3 vector)
        {
            return new Vector3(vector.x, 0f, vector.z);
        }

        public static Vector3 ReflectVectorYZero(Vector3 vector, Vector3 axis)
        {
            var vector2 = SetYToZero(vector);
            var axis2 = SetYToZero(axis);

            axis2.Normalize();

            var dot = Vector3.Dot(vector2, axis2);

            var projectVector = dot * axis2;
            Debug.Log($"projectVector:{projectVector}");

            var perpendicularVector = vector2 - projectVector;
            Debug.Log($"perpendicularVector:{perpendicularVector}");

            return projectVector - perpendicularVector;
        }


        public static GameObject ChooseRandomObject(IEnumerable<GameObject> gameObjects)
        {
            if (gameObjects == null) throw new ArgumentNullException(nameof(gameObjects));
            var count = gameObjects.Count();
            if (count == 0) throw new ArgumentException("No reference in gameObject argument", nameof(gameObjects));
            var index = Random.Range(0, count);
            return gameObjects.ElementAt(index);
        }


        //https://gist.github.com/0V/10fe01dd953db7aa8d7e

        /// <summary>
        ///     <para>
        ///         ガウス＝ザイデル法（Gauss-Seidel method）
        ///     </para>
        ///     <para>
        ///         解が収束するのは
        ///         ・対角有利(diagonal dominant, 対角要素の絶対値>その行の他の要素の絶対値の和)
        ///         ・係数行列が対称(symmetric)かつ正定(positive definite)
        ///         ・Σ_j |a_ij/a_ii| &lt; 1 (i = 1～n, j != i)
        ///     </para>
        /// </summary>
        /// <param name="squareMatrix">n×nの係数行列</param>
        /// <param name="constantVector">n×1の定数項(b)の行列(定数項ベクトル)</param>
        /// <param name="maxIterator">最大反復数</param>
        /// <param name="eps">許容誤差</param>
        /// <param name="AbsoluteError">
        ///     [option]収束判定で絶対誤差と相対誤差のどちらを用いるか示す。
        ///     真のときは絶対誤差、偽のときは相対誤差を用いる。
        /// </param>
        /// <returns>解の行列</returns>
        public static IterativeResult GaussSeidel(double[,] squareMatrix, double[] constantVector, int maxIterator,
            double eps, bool AbsoluteError = true)
        {
            if (squareMatrix.GetLength(0) != squareMatrix.GetLength(1))
                throw new ArgumentException("引き数の係数行列が正方行列でありません。", "A");
            if (squareMatrix.GetLength(0) != constantVector.Length)
                throw new ArgumentException("引き数の定数項行列が係数行列の大きさと一致しません。");

            // 行列の大きさ
            var n = squareMatrix.GetLength(0);
            // 解。初期値はすべて0
            var solution = new double[n];
            // 誤差
            var e = 0.0;
            // 現在の反復回数
            int k;

            double tmp;

            for (k = 0; k < maxIterator; ++k)
            {
                // 現在の値を代入して次の解候補を計算
                e = 0.0;
                for (var i = 0; i < n; ++i)
                {
                    tmp = solution[i];
                    solution[i] = constantVector[i];
                    for (var j = 0; j < n; ++j) solution[i] -= j != i ? squareMatrix[i, j] * solution[j] : 0.0;
                    solution[i] /= squareMatrix[i, i];

                    if (AbsoluteError)
                        // 絶対誤差
                        e += Math.Abs(tmp - solution[i]);
                    else
                        // 相対誤差
                        e += Math.Abs((tmp - solution[i]) / tmp);
                }

                // 収束判定
                if (e <= eps) break;
            }

            return new IterativeResult(solution, k, e);
        }

        public struct IterativeResult
        {
            public IterativeResult(double[] solution, int iterator, double error)
            {
                Solution = solution;
                Iterator = iterator;
                Error = error;
            }

            /// <summary>
            ///     解
            /// </summary>
            public double[] Solution { get; set; }

            /// <summary>
            ///     反復回数
            /// </summary>
            public int Iterator { get; set; }

            /// <summary>
            ///     誤差
            /// </summary>
            public double Error { get; set; }
        }
    }
}