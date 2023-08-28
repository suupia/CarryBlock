using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Spawners;
using Fusion;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    public class WallPresenterBuilder  : IPresenterBuilder
    {
        [Inject] NetworkRunner _runner;
        IEnumerable<WallPresenterNet> _tilePresenters = new List<WallPresenterNet>();

        readonly int _wallHorizontalNum = 3;
        readonly int _wallVerticalNum = 2;

        [Inject]
        public WallPresenterBuilder()
        {
        }

        public void Build(EntityGridMap map)
        {
            var wallPresenterSpawner = new WallPresenterSpawner(_runner);
            var wallPresenters = new List<WallPresenterNet>();

            // 以前のWallPresenterを削除
            DestroyWallPresenter();

            // WallPresenterをスポーンさせる
            var expandedMap = new SquareGridMap(map.Width + 2 * _wallHorizontalNum, map.Height + 2 * _wallVerticalNum);
            for (int i = 0; i < expandedMap.Length; i++)
            {
                var gridPos = expandedMap.ToVector(i);
                var convertedGridPos = new Vector2Int(gridPos.x - _wallHorizontalNum, gridPos.y - _wallVerticalNum);
                if (map.IsInDataRangeArea(convertedGridPos)) continue;
                if (IsBlockNotPlacedOnRightEdge(map, convertedGridPos)) continue;
                var worldPos = GridConverter.GridPositionToWorldPosition(convertedGridPos);
                var wallPresenter = wallPresenterSpawner.SpawnPrefab(worldPos, Quaternion.identity);
                wallPresenters.Add(wallPresenter);
                

            }
            
            _tilePresenters = wallPresenters;
        }

        void DestroyWallPresenter()
        {
            // マップの大きさが変わっても対応できるようにDestroyが必要
            // ToDo: マップの大きさを変えてテストをする 

            foreach (var tilePresenter in _tilePresenters)
            {
                _runner.Despawn(tilePresenter.Object);
            }

            _tilePresenters = new List<WallPresenterNet>();
        }

        bool IsBlockNotPlacedOnRightEdge(EntityGridMap map, Vector2Int gridPos)
        {
            if(gridPos.y < 0 || gridPos.y > map.Height - 1) return false;
            if (gridPos.x >= map.Width)
            {
                if (map.GetSingleEntityList<IBlock>(new Vector2Int(gridPos.x, map.Width - 1)).Count == 0) return true;
            }

            return false;
        }
    }
}