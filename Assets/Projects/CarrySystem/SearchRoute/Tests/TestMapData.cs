using System.Collections.Generic;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.SearchRoute.Scripts;
using NUnit.Framework;
using Projects.CarrySystem.SearchRoute.Scripts;
using UnityEngine;

namespace Projects.CarrySystem.RoutingAlgorithm.Tests
{
    public class Map7X7A
    {
        public NumericGridMap Map { get; }
        public Vector2Int Startpos => new Vector2Int(0, 2);
        public Vector2Int EndPos => new Vector2Int(6, 6);
        public IReadOnlyList<(int, int)> Walls { get; }
        public IReadOnlyList<(int, int)> WallsIncludeStart { get; }
        public IReadOnlyList<(int, int)> WallsIncludeEnd { get; }

        public Map7X7A()
        {
            int width = 7;
            int height = 7;
            var initValue = -1;
            var edgeValue = -8;
            var outOfRangeValue = -88;
            Map = new NumericGridMap(width, height, initValue, edgeValue, outOfRangeValue);

            Walls = new List<(int, int)>() { (3, 1), (4, 1), (5, 1), (6, 1), (3, 0) };
            var tmpWallsIncludeStart = new List<(int, int)>(Walls);
            tmpWallsIncludeStart.Add((Startpos.x, Startpos.y));
            WallsIncludeStart = tmpWallsIncludeStart;
            var tmpWallsIncludeEnd = new List<(int, int)>(Walls);
            tmpWallsIncludeEnd.Add((EndPos.x, EndPos.y));
            WallsIncludeEnd = tmpWallsIncludeEnd;
        }
    }
    public class Map7X7B
    {
        public NumericGridMap Map { get; }
        public Vector2Int Startpos => new Vector2Int(1, 1);
        public Vector2Int EndPos => new Vector2Int(4, 5);
        public IReadOnlyList<(int, int)> Walls { get; }
        public IReadOnlyList<(int, int)> WallsIncludeStart { get; }
        public IReadOnlyList<(int, int)> WallsIncludeEnd { get; }

        public Map7X7B()
        {
            int width = 7;
            int height = 7;
            var initValue = -1;
            var edgeValue = -8;
            var outOfRangeValue = -88;
            Map = new NumericGridMap(width, height, initValue, edgeValue, outOfRangeValue);

            Walls = new List<(int, int)>() { (3, 0), (3, 1), (4, 1), (4, 2), (4, 3), (3, 4), (3, 5), (3, 6) };
            var tmpWallsIncludeStart = new List<(int, int)>(Walls);
            tmpWallsIncludeStart.Add((Startpos.x, Startpos.y));
            WallsIncludeStart = tmpWallsIncludeStart;
            var tmpWallsIncludeEnd = new List<(int, int)>(Walls);
            tmpWallsIncludeEnd.Add((EndPos.x, EndPos.y));
            WallsIncludeEnd = tmpWallsIncludeEnd;
        }
    }
    public class Map7X7C
    {
        public NumericGridMap Map { get; }
        public Vector2Int Startpos => new Vector2Int(0, 5);
        public Vector2Int EndPos => new Vector2Int(6, 0);
        public IReadOnlyList<(int, int)> Walls { get; }
        public IReadOnlyList<(int, int)> WallsIncludeStart { get; }
        public IReadOnlyList<(int, int)> WallsIncludeEnd { get; }

        public Map7X7C()
        {
            int width = 7;
            int height = 7;
            var initValue = -1;
            var edgeValue = -8;
            var outOfRangeValue = -88;
            Map = new NumericGridMap(width, height, initValue, edgeValue, outOfRangeValue);

            Walls = new List<(int, int)>() { (1, 1), (2, 4), (2, 5), (2, 6), (4, 0), (4, 1), (4, 2), (4, 3), (4, 4) };
            var tmpWallsIncludeStart = new List<(int, int)>(Walls);
            tmpWallsIncludeStart.Add((Startpos.x, Startpos.y));
            WallsIncludeStart = tmpWallsIncludeStart;
            var tmpWallsIncludeEnd = new List<(int, int)>(Walls);
            tmpWallsIncludeEnd.Add((EndPos.x, EndPos.y));
            WallsIncludeEnd = tmpWallsIncludeEnd;
        }
    }
}