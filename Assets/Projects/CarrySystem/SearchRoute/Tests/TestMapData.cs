﻿using System.Collections.Generic;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.SearchRoute.Scripts;
using NUnit.Framework;
using UnityEngine;

namespace Carry.CarrySystem.RoutingAlgorithm.Tests
{
    public class Map7X7A
    {
        public NumericGridMap Map { get; }
        public Vector2Int StartPos => new Vector2Int(0, 2);
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
            tmpWallsIncludeStart.Add((StartPos.x, StartPos.y));
            WallsIncludeStart = tmpWallsIncludeStart;
            var tmpWallsIncludeEnd = new List<(int, int)>(Walls);
            tmpWallsIncludeEnd.Add((EndPos.x, EndPos.y));
            WallsIncludeEnd = tmpWallsIncludeEnd;
        }
    }

    public class Map7X7B
    {
        public NumericGridMap Map { get; }
        public Vector2Int StartPos => new Vector2Int(1, 1);
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
            tmpWallsIncludeStart.Add((StartPos.x, StartPos.y));
            WallsIncludeStart = tmpWallsIncludeStart;
            var tmpWallsIncludeEnd = new List<(int, int)>(Walls);
            tmpWallsIncludeEnd.Add((EndPos.x, EndPos.y));
            WallsIncludeEnd = tmpWallsIncludeEnd;
        }
    }

    public class Map7X7C
    {
        public NumericGridMap Map { get; }
        public Vector2Int StartPos => new Vector2Int(0, 5);
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
            tmpWallsIncludeStart.Add((StartPos.x, StartPos.y));
            WallsIncludeStart = tmpWallsIncludeStart;
            var tmpWallsIncludeEnd = new List<(int, int)>(Walls);
            tmpWallsIncludeEnd.Add((EndPos.x, EndPos.y));
            WallsIncludeEnd = tmpWallsIncludeEnd;
        }
    }

    public class Map10X8A
    {
        public NumericGridMap Map { get; }
        public Vector2Int StartPos => new Vector2Int(1, 3);
        public Vector2Int EndPos => new Vector2Int(7, 6);
        public IReadOnlyList<(int, int)> Walls { get; }
        public IReadOnlyList<(int, int)> WallsIncludeStart { get; }
        public IReadOnlyList<(int, int)> WallsIncludeEnd { get; }

        public Map10X8A()
        {
            int width = 10;
            int height = 8;
            var initValue = -1;
            var edgeValue = -8;
            var outOfRangeValue = -88;
            Map = new NumericGridMap(width, height, initValue, edgeValue, outOfRangeValue);

            Walls = new List<(int, int)>() { (6, 0), (7, 0), (8, 3), (0, 5), (1, 5), (2, 5), (3, 5), (3, 6), (3, 7) };
            var tmpWallsIncludeStart = new List<(int, int)>(Walls);
            tmpWallsIncludeStart.Add((StartPos.x, StartPos.y));
            WallsIncludeStart = tmpWallsIncludeStart;
            var tmpWallsIncludeEnd = new List<(int, int)>(Walls);
            tmpWallsIncludeEnd.Add((EndPos.x, EndPos.y));
            WallsIncludeEnd = tmpWallsIncludeEnd;
        }
    }
    
    public class Map10X8B
    {
        public NumericGridMap Map { get; }
        public Vector2Int StartPos => new Vector2Int(1, 6);
        public Vector2Int EndPos => new Vector2Int(8, 6);
        public IReadOnlyList<(int, int)> Walls { get; }
        public IReadOnlyList<(int, int)> WallsIncludeStart { get; }
        public IReadOnlyList<(int, int)> WallsIncludeEnd { get; }

        public Map10X8B()
        {
            int width = 10;
            int height = 8;
            var initValue = -1;
            var edgeValue = -8;
            var outOfRangeValue = -88;
            Map = new NumericGridMap(width, height, initValue, edgeValue, outOfRangeValue);

            Walls = new List<(int, int)>() { (3,5),(3,6),(5,0),(6,1),(7,2),(7,3) };
            var tmpWallsIncludeStart = new List<(int, int)>(Walls);
            tmpWallsIncludeStart.Add((StartPos.x, StartPos.y));
            WallsIncludeStart = tmpWallsIncludeStart;
            var tmpWallsIncludeEnd = new List<(int, int)>(Walls);
            tmpWallsIncludeEnd.Add((EndPos.x, EndPos.y));
            WallsIncludeEnd = tmpWallsIncludeEnd;
        }
    }
    
    public class Map11X8A
    {
        public NumericGridMap Map { get; }
        public Vector2Int StartPos => new Vector2Int(1, 1);
        public Vector2Int EndPos => new Vector2Int(9, 6);
        public IReadOnlyList<(int, int)> Walls { get; }
        public IReadOnlyList<(int, int)> WallsIncludeStart { get; }
        public IReadOnlyList<(int, int)> WallsIncludeEnd { get; }

        public Map11X8A()
        {
            int width = 11;
            int height = 8;
            var initValue = -1;
            var edgeValue = -8;
            var outOfRangeValue = -88;
            Map = new NumericGridMap(width, height, initValue, edgeValue, outOfRangeValue);

            Walls = new List<(int, int)>() { (3,0),(3,1),(3,2),(3,7),(5,6),(7,3),(7,4) };
            var tmpWallsIncludeStart = new List<(int, int)>(Walls);
            tmpWallsIncludeStart.Add((StartPos.x, StartPos.y));
            WallsIncludeStart = tmpWallsIncludeStart;
            var tmpWallsIncludeEnd = new List<(int, int)>(Walls);
            tmpWallsIncludeEnd.Add((EndPos.x, EndPos.y));
            WallsIncludeEnd = tmpWallsIncludeEnd;
        }
    }
}