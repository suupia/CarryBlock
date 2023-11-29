﻿#nullable enable
using Carry.CarrySystem.Map.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Map.Scripts
{
    public sealed class SquareGridCoordinate
    {
        int Width { get; }
        int Height { get; }

        public SquareGridCoordinate(int width, int height)
        {
            Width = width;
            Height = height;
        }

        // 変換メソッド
        public int ToSubscript(int x, int y)
        {
            if (IsOutOfIncludeEdgeArea(x, y))
            {
                Debug.LogError($"IsOutOfIncludeEdgeArea({x}, {y})がtrueです");
                return -1;
            }

            return x + Width * y;
        }

        public Vector2Int ToVector(int subscript)
        {
            int x = subscript % Width;
            int y = subscript / Width;
            return new Vector2Int(x, y);
        }


        /// <summary>
        /// dataArea(座標(0,0)～(mapWidth-1,mapHeight-1)のデータが存在する領域)の内側であればtrueを返す
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsInDataArea(int x, int y)
        {
            return !IsOutOfDataArea(x, y);
        }

        public bool IsInDataArea(Vector2Int vector)
        {
            return IsInDataArea(vector.x, vector.y);
        }

        /// <summary>
        /// edgeArea(データは存在しないが、_initValueを返す領域)の内側であればtrueを返す
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsInDataOrEdgeArea(int x, int y)
        {
            return !IsOutOfIncludeEdgeArea(x, y);
        }

        public bool IsInDataOrEdgeArea(Vector2Int vector)
        {
            return IsInDataOrEdgeArea(vector.x, vector.y);
        }


        // 子クラスでエラーを出すための判定用メソッド
        // クライアント側でIsInIncludeEdgeAreaやIsInDataRangeAreaの呼び出しを忘れないようにエラーを投げるときに使う

        /// <summary>
        /// edgeArea(データは存在しないが、_initValueを返す領域)の外側であればtrueを返す
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsOutOfIncludeEdgeArea(int x, int y)
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
        public bool IsOnTheEdgeArea(int x, int y)
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
        public bool IsOutOfDataArea(int x, int y)
        {
            return IsOutOfIncludeEdgeArea(x, y) || IsOnTheEdgeArea(x, y);
        }
    }
}