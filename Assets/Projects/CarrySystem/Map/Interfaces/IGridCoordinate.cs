#nullable enable

using UnityEngine;

namespace Carry.CarrySystem.Map.Interfaces
{
    public interface IGridCoordinate
    {
        // DataArea is a area that has data. ex) (0,0)～(mapWidth-1,mapHeight-1)
        // EdgeArea is a area that has no data but has _initValue.
        // OutOfIncludeEdgeArea is a area that has no data and has no _initValue.
        
        public int Width { get; }
        public int Height { get; }
        public int Length => Width * Height;

        // Convert subscript to vector.
        public int ToSubscript(int x, int y);

        // Convert vector to subscript.
        public Vector2Int ToVector(int subscript);

        // This method must be implemented in the child class.
        public bool IsInDataArea(int x, int y);

        // This method must be implemented in the child class.
        public bool IsInDataOrEdgeArea(int x, int y);
        
        // These are default implementation.
        public bool IsOnTheEdgeArea(int x, int y)
        {
            return IsInDataOrEdgeArea(x, y) && !IsInDataArea(x, y);
        }

        public bool IsOutOfIncludeEdgeArea(int x, int y)
        {
            return !IsInDataOrEdgeArea(x, y);
        }

        public bool IsOutOfDataArea(int x, int y)
        {
            return IsOutOfIncludeEdgeArea(x, y) || IsOnTheEdgeArea(x, y);
        }

        #region Overload

        public bool IsInDataArea(Vector2Int vector)
        {
            return IsInDataArea(vector.x, vector.y);
        }

        public bool IsInDataOrEdgeArea(Vector2Int vector)
        {
            return IsInDataOrEdgeArea(vector.x, vector.y);
        }

        public bool IsOnTheEdgeArea(Vector2Int vector)
        {
            return IsOnTheEdgeArea(vector.x, vector.y);
        }

        public bool IsOutOfIncludeEdgeArea(Vector2Int vector)
        {
            return IsOutOfIncludeEdgeArea(vector.x, vector.y);
        }

        public bool IsOutOfDataArea(Vector2Int vector)
        {
            return IsOutOfDataArea(vector.x, vector.y);
        }

        #endregion
    }
}