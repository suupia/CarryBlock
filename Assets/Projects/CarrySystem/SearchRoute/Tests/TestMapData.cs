using System.Collections.Generic;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.SearchRoute.Scripts;
using NUnit.Framework;
using Projects.CarrySystem.SearchRoute.Scripts;
using UnityEngine;

namespace Projects.CarrySystem.RoutingAlgorithm.Tests
{
    public class TestMapData
    {
        public void Test()
        {
            int width = 7;
            int height = 7;
            var initValue = -1;
            var edgeValue = -8;
            var outOfRangeValue = -88;
            var map = new NumericGridMap(width, height, initValue, edgeValue, outOfRangeValue);

            var startPos = new Vector2Int(0, 5);
            var endPos = new Vector2Int(6, 0);
            var walls = new List<(int, int)>()
                { (1, 1), (2, 4), (2, 5), (2, 6), (4, 0), (4, 1), (4, 2), (4, 3), (4, 4) };
            var wallsIncludeStart = new List<(int, int)>(walls);
            wallsIncludeStart.Add((startPos.x, startPos.y));
            var wallsIncludeEnd = new List<(int, int)>(walls);
            wallsIncludeEnd.Add((endPos.x, endPos.y));
        }
    }

    public class Map7X7C
    {
        public NumericGridMap Map => _map;
        public Vector2Int Startpos => new Vector2Int(0, 5);
        public Vector2Int EndPos => new Vector2Int(6, 0);
        public IReadOnlyList<(int, int)> Walls => _walls;
        public IReadOnlyList<(int, int)> WallsIncludeStart => _wallsIncludeStart;
        public IReadOnlyList<(int, int)> WallsIncludeEnd => _wallsIncludeEnd;
        
        NumericGridMap _map;
        IReadOnlyList<(int, int)> _walls;
        IReadOnlyList<(int, int)> _wallsIncludeStart;
        IReadOnlyList<(int, int)> _wallsIncludeEnd;

        public Map7X7C()
        {
            int width = 7;
            int height = 7;
            var initValue = -1;
            var edgeValue = -8;
            var outOfRangeValue = -88;
            _map = new NumericGridMap(width, height, initValue, edgeValue, outOfRangeValue);

            _walls = new List<(int, int)>() { (1, 1), (2, 4), (2, 5), (2, 6), (4, 0), (4, 1), (4, 2), (4, 3), (4, 4) };
            var tmpWallsIncludeStart = new List<(int, int)>(_walls);
            tmpWallsIncludeStart.Add((Startpos.x, Startpos.y));
            _wallsIncludeStart = tmpWallsIncludeStart;
            var tmpWallsIncludeEnd = new List<(int, int)>(_walls);
            tmpWallsIncludeEnd.Add((EndPos.x, EndPos.y));
            _wallsIncludeEnd = tmpWallsIncludeEnd;
        }
    }
}