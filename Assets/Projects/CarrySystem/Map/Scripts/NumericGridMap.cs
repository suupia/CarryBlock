﻿using UnityEngine;
using UnityEngine.Assertions;
#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class NumericGridMap
    {
        //数字だけを格納することができるマップ
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int GetLength() => _values.Length;
        readonly int _edgeValue;
        readonly int _outOfRangeValue;


        readonly long[] _values;

        // dataArea, edgeArea, outOfRangeArea の３つの領域に分けて考える

        public NumericGridMap(int width, int height, int initValue, int edgeValue, int outOfRangeValue)
        {
            // Debug.Log($"width:{width}, height:{height}");
            Assert.IsTrue(width > 0 && height > 0, "Mapの幅または高さが0以下になっています");
            Width = width;
            Height = height;
            _values = new long[width * height];
            _edgeValue = edgeValue;
            _outOfRangeValue = outOfRangeValue;
            
            FillAll(initValue);
        }

        //初期化で利用
        public void FillAll(int value)
        {
            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] = value;
            }
        }

        // Getter
        public long GetValue(int index)
        {
            if (index < 0 || index > _values.Length)
            {
                Debug.LogError("領域外の値を習得しようとしました");
                return _outOfRangeValue;
            }

            return _values[index];
        }

        public long GetValue(int x, int y)
        {
            if (IsOutOfIncludeEdgeArea(x, y))
            {
                //edgeの外側
                Debug.LogError($"IsOutOfEdgeArea({x},{y})がtrueです");
                return _outOfRangeValue;
            }

            if (IsOnTheEdgeArea(x, y))
            {
                //edgeの上
                //データは存在しないが、判定のために初期値を使いたい場合
                return _edgeValue;
            }

            //Debug.Log($"ToSubscript:{ToSubscript(x,y)}, x:{x}, y:{y}, IsOnTheEdge({x},{y}):{IsOnTheEdge(x,y)}, IsOutOfEdge({x},{y}):{IsOutOfEdge(x,y)}, Width:{Width}, Height:{Height}");
            return _values[ToSubscript(x, y)];
        }

        public long GetValue(Vector2Int vector)
        {
            return GetValue(vector.x, vector.y);
        }

        public Vector2Int GetVectorFromIndex(int index)
        {
            return ToVector(index);
        }

        public int GetIndexFromVector(Vector2Int vector)
        {
            if (IsOutOfIncludeEdgeArea(vector.x, vector.y))
            {
                Debug.LogError($"IsOutOfIncludeEdgeArea({vector.x}, {vector.y})がtrueです");
                return -1;
            }

            return ToSubscript(vector.x, vector.y);
        }


        //Setter
        public void SetValue(int index, long value)
        {
            if (index < 0 || index > _values.Length - 1)
            {
                Debug.LogError("領域外の値を習得しようとしました");
                return;
            }

            _values[index] = value;
        }

        public void SetValue(int x, int y, long value)
        {
            if (IsOutOfDataRangeArea(x, y))
            {
                // Debug.Log($"IsOutOfDataRangeArea({x},{y})がtrueです");
                return;
            }
            _values[ToSubscript(x, y)] = value;
        }

        public void SetValue(Vector2Int vector, long value)
        {
            SetValue(vector.x, vector.y, value);
        }

        public void AdditionSetValue(Vector2Int vector, long value)
        {
            var x = vector.x;
            var y = vector.y;
            SetValue(x, y, GetValue(x, y) + value);
        }

        public void MultiplySetValue(Vector2Int vector, int value)
        {
            var x = vector.x;
            var y = vector.y;

            if (IsOutOfIncludeEdgeArea(x, y))
            {
                Debug.LogError($"IsOutOfEdgeArea({x},{y})がtrueです");
                return;
            }

            _values[ToSubscript(x, y)] *= value;
        }

        public void DivisionalSetValue(Vector2Int vector, int value)
        {
            var x = vector.x;
            var y = vector.y;

            if (IsOutOfIncludeEdgeArea(x, y))
            {
                Debug.LogError($"IsOutOfEdgeArea({x},{y})がtrueです");
                return;
            }

            if (GetValue(x, y) % value != 0)
            {
                Debug.LogError($"DivisionalSetValue(vector:{vector},value:{value})で余りが出たため実行できません");
                return;
            }

            _values[ToSubscript(x, y)] /= value;
        }
        
        // クライアント側で使用することを想定する判定用関数
        
        /// <summary>
        /// edgeArea(データは存在しないが、_initValueを返す領域)の内側であればtrueを返す
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsInIncludeEdgeArea(int x, int y)
        {
            return !IsOutOfIncludeEdgeArea(x, y);
        }
        public bool IsInIncludeEdgeArea(Vector2Int vector)
        {
            return IsInIncludeEdgeArea(vector.x, vector.y);
        }

        
        /// <summary>
        /// dataArea(座標(0,0)～(mapWidth-1,mapHeight-1)のデータが存在する領域)の内側であればtrueを返す
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsInDataRangeArea(int x, int y)
        {
            return !IsOutOfDataRangeArea(x, y);
        }
        public bool IsInDataRangeArea(Vector2Int vector)
        {
            return IsInDataRangeArea(vector.x, vector.y);
        }


        //添え字を変換する
        public int ToSubscript(int x, int y)
        {
            return x + Width * y;
        }

        public Vector2Int ToVector(int subscript)
        {
            int x = subscript % Width;
            int y = subscript / Width;
            return new Vector2Int(x, y);
        }
        


        // エラーを出すための判定用関数
        // クライアント側でIsInIncludeEdgeAreaやIsInDataRangeAreaの呼び出しを忘れないようにエラーを投げるときに使う
        
        /// <summary>
        /// edgeArea(データは存在しないが、_initValueを返す領域)の外側であればtrueを返す
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected bool IsOutOfIncludeEdgeArea(int x, int y)
        {
            if (x < -1 || Width < x) return true;
            if (y < -1 || Height < y) return true;
            return false;
        }

        /// <summary>
        /// edgeArea(データは存在しないが、_initValueを返す領域)の上であればtrueを返す
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected bool IsOnTheEdgeArea(int x, int y)
        {
            if ((x == -1 || x == Width) && (-1 <= y && y <= Height)) return true; //左右の両端
            if ((y == -1 || y == Height) && (-1 <= x && x <= Width)) return true; //上下の両端
            return false;
        }

        /// <summary>
        /// dataArea(座標(0,0)～(mapWidth-1,mapHeight-1)のデータが存在する領域)の外側であればtrueを返す
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected bool IsOutOfDataRangeArea(int x, int y)
        {
            return IsOutOfIncludeEdgeArea(x, y) || IsOnTheEdgeArea(x, y);
        }
    }
}