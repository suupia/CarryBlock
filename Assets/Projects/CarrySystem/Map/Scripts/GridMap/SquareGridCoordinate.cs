#nullable enable
using Carry.CarrySystem.Map.Interfaces;
using UnityEngine;
using Debug = System.Diagnostics.Debug;
namespace Carry.CarrySystem.Map.Scripts
{
    public sealed class SquareGridCoordinate : IGridCoordinate
    {
        public int Width { get; }
        public int Height { get; }
        public int Length => Width * Height;

        public SquareGridCoordinate(int width, int height)
        {
            Width = width;
            Height = height;
        }
        
        public int ToSubscript(int x, int y)
        {
            Debug.Assert(IsInDataOrEdgeArea(x,y));

            return x + Width * y;
        }

        public Vector2Int ToVector(int subscript)
        {
            int x = subscript % Width;
            int y = subscript / Width;
            return new Vector2Int(x, y);
        }
        
        public bool IsInDataArea(int x, int y)
        {
            if (x < 0 || Width <= x) return false;
            if (y < 0 || Height <= y) return false;
            return true;
        }

        public bool IsInDataOrEdgeArea(int x, int y)
        {
            if (x < -1 || Width < x) return false;
            if (y < -1 || Height < y) return false;
            return true;
        }

        public bool IsOutOfIncludeEdgeArea(int x, int y)
        {
            return !IsInDataOrEdgeArea(x, y);
        }
    }
}