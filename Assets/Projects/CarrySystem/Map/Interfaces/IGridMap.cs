using UnityEngine;

namespace Carry.CarrySystem.Map.Interfaces
{
    public interface IGridMap
    {
        public int Width { get; }
        public int Height { get; }
        public int Length { get; }

        public int ToSubscript(int x, int y);
        public Vector2Int ToVector(int subscript);
    }
}