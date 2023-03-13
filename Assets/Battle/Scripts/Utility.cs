using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
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

        float dot = Vector3.Dot(vector2, axis2);

        Vector3 projectVector = dot * axis2;
        Debug.Log($"projectVector:{projectVector}");

        Vector3 perpendicularVector = vector2 - projectVector;
        Debug.Log($"perpendicularVector:{perpendicularVector}");

        return projectVector- perpendicularVector;
    }



    //https://gist.github.com/0V/10fe01dd953db7aa8d7e

    /// <summary>
    /// <para>
    ///  �K�E�X���U�C�f���@�iGauss-Seidel method�j
    /// </para>
    /// <para>
    /// ������������̂�
    ///    �E�Ίp�L��(diagonal dominant, �Ίp�v�f�̐�Βl>���̍s�̑��̗v�f�̐�Βl�̘a)
    ///    �E�W���s�񂪑Ώ�(symmetric)������(positive definite)
    ///    �E��_j |a_ij/a_ii| &lt; 1 (i = 1�`n, j != i) 
    /// </para>
    /// </summary>
    /// <param name="squareMatrix">n�~n�̌W���s��</param>
    /// <param name="constantVector">n�~1�̒萔��(b)�̍s��(�萔���x�N�g��)</param>
    /// <param name="maxIterator">�ő唽����</param>
    /// <param name="eps">���e�덷</param>
    /// <param name="AbsoluteError">[option]��������Ő�Ό덷�Ƒ��Ό덷�̂ǂ����p���邩�����B
    /// �^�̂Ƃ��͐�Ό덷�A�U�̂Ƃ��͑��Ό덷��p����B</param>
    /// <returns>���̍s��</returns>
    public static IterativeResult GaussSeidel(double[,] squareMatrix, double[] constantVector, int maxIterator, double eps, bool AbsoluteError = true)
    {
        if (squareMatrix.GetLength(0) != squareMatrix.GetLength(1))
        {
            throw new ArgumentException("�������̌W���s�񂪐����s��ł���܂���B", "A");
        }
        if (squareMatrix.GetLength(0) != constantVector.Length)
        {
            throw new ArgumentException("�������̒萔���s�񂪌W���s��̑傫���ƈ�v���܂���B");
        }

        // �s��̑傫��
        int n = squareMatrix.GetLength(0);
        // ���B�����l�͂��ׂ�0
        double[] solution = new double[n];
        // �덷
        double e = 0.0;
        // ���݂̔�����
        int k;

        double tmp;

        for (k = 0; k < maxIterator; ++k)
        {
            // ���݂̒l�������Ď��̉������v�Z
            e = 0.0;
            for (int i = 0; i < n; ++i)
            {
                tmp = solution[i];
                solution[i] = constantVector[i];
                for (int j = 0; j < n; ++j)
                {
                    solution[i] -= (j != i ? squareMatrix[i, j] * solution[j] : 0.0);
                }
                solution[i] /= squareMatrix[i, i];

                if (AbsoluteError)
                {
                    // ��Ό덷
                    e += Math.Abs(tmp - solution[i]);
                }
                else
                {
                    // ���Ό덷
                    e += Math.Abs((tmp - solution[i]) / tmp);
                }
            }
            // ��������
            if (e <= eps)
            {
                break;
            }
        }

        return new IterativeResult(solution, k, e);
    }

    public struct IterativeResult
    {
        public IterativeResult(double[] solution, int iterator, double error)
        {
            this.Solution = solution;
            this.Iterator = iterator;
            this.Error = error;

        }

        /// <summary>
        /// ��
        /// </summary>
        public double[] Solution { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public int Iterator { get; set; }

        /// <summary>
        /// �덷
        /// </summary>
        public double Error { get; set; }
    }
}
