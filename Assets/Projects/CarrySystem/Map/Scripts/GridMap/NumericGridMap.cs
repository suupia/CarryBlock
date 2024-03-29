﻿using System.Text;
using Carry.CarrySystem.Map.Interfaces;
using UnityEngine;
using UnityEngine.Assertions;
#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    //数字だけを格納することができるマップ
    public sealed class NumericGridMap : IGridMap 
    {
        public int Width { get; }
        public int Height { get; }
        public int Length => Width * Height;
        
        public int ToSubscript(int x, int y) => _coordinate.ToSubscript(x, y);
        public Vector2Int ToVector(int subscript) => _coordinate.ToVector(subscript);

        readonly IGridCoordinate _coordinate;
        
        readonly int _edgeValue;
        readonly int _outOfRangeValue;

        readonly long[] _values;

        public NumericGridMap(int width, int height, int initValue, int edgeValue, int outOfRangeValue)
        {
            Width = width;
            Height = height;
            _coordinate = new SquareGridCoordinate(width, height);
            Assert.IsTrue(width > 0 && height > 0, "Mapの幅または高さが0以下になっています");
            _values = new long[width * height];
            _edgeValue = edgeValue;
            _outOfRangeValue = outOfRangeValue;
            
            FillAll(initValue);
        }
        
        public bool IsInDataArea(int x, int y)
        {
            return _coordinate.IsInDataArea(x, y);
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
            if (_coordinate. IsOutOfIncludeEdgeArea(x, y))
            {
                //edgeの外側
                Debug.LogError($"IsOutOfEdgeArea({x},{y})がtrueです");
                return _outOfRangeValue;
            }

            if (_coordinate. IsOnTheEdgeArea(x, y))
            {
                //edgeの上
                //データは存在しないが、判定のために初期値を使いたい場合
                return _edgeValue;
            }

            //Debug.Log($"ToSubscript:{ToSubscript(x,y)}, x:{x}, y:{y}, IsOnTheEdge({x},{y}):{IsOnTheEdge(x,y)}, IsOutOfEdge({x},{y}):{IsOutOfEdge(x,y)}, Width:{Width}, Height:{Height}");
            return _values[_coordinate. ToSubscript(x, y)];
        }

        public long GetValue(Vector2Int vector)
        {
            return GetValue(vector.x, vector.y);
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
            if (_coordinate. IsOutOfDataArea(x, y))
            {
                // Debug.Log($"IsOutOfDataRangeArea({x},{y})がtrueです");
                return;
            }
            _values[_coordinate. ToSubscript(x, y)] = value;
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

            if (_coordinate. IsOutOfIncludeEdgeArea(x, y))
            {
                Debug.LogError($"IsOutOfEdgeArea({x},{y})がtrueです");
                return;
            }

            _values[_coordinate. ToSubscript(x, y)] *= value;
        }

        public void DivisionalSetValue(Vector2Int vector, int value)
        {
            var x = vector.x;
            var y = vector.y;

            if (_coordinate. IsOutOfIncludeEdgeArea(x, y))
            {
                Debug.LogError($"IsOutOfEdgeArea({x},{y})がtrueです");
                return;
            }

            if (GetValue(x, y) % value != 0)
            {
                Debug.LogError($"DivisionalSetValue(vector:{vector},value:{value})で余りが出たため実行できません");
                return;
            }

            _values[_coordinate. ToSubscript(x, y)] /= value;
        }


        /// <summary>
        /// デバッグ用
        /// </summary>
        /// <param name="map"></param>
        public void DebugMapValues()
        {
            //デバッグ用
            StringBuilder debugCell = new StringBuilder();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    long value = GetValue(x, Height - y - 1);
                    debugCell.AppendFormat("{0,4},",
                        (value >= 0 ? " " : "") + value.ToString("D2")); // 桁数をそろえるために0を追加していると思う
                }
            
                debugCell.AppendLine();
            }
            Debug.Log($"すべてのGetValue()の結果は\n{debugCell}");
        }
        
    }
}