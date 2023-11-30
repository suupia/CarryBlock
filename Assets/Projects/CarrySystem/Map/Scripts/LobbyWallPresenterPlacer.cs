using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Spawners.Scripts;
using Fusion;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    public class LobbyWallPresenterPlacer : IPresenterPlacer
    {
        [Inject] NetworkRunner _runner;
        IEnumerable<IPresenterMono> _tilePresenters = new List<IPresenterMono>();

        readonly int _wallHorizontalNum = 10;
        readonly int _wallVerticalNum = 10;

        [Inject]
        public LobbyWallPresenterPlacer()
        {
        }

        public void Place(EntityGridMap map)
        {
            var wallPresenterSpawner = new WallPresenterNetSpawner(_runner);
            var wallPresenters = new List<IPresenterMono>();

            // 以前のWallPresenterを削除
            DestroyWallPresenter();

            // WallPresenterをスポーンさせる
            var expandedCoordinate = new SquareGridCoordinate(map.Width + 2 * _wallHorizontalNum, map.Height + 2 * _wallVerticalNum);
            for (int i = 0; i < expandedCoordinate.Length; i++)
            {
                var gridPos = expandedCoordinate.ToVector(i);
                var convertedGridPos = new Vector2Int(gridPos.x - _wallHorizontalNum, gridPos.y - _wallVerticalNum);
                if (map.IsInDataArea(convertedGridPos)) continue;
                var worldPos = GridConverter.GridPositionToWorldPosition(convertedGridPos);
                var wallPresenter = wallPresenterSpawner.SpawnPrefab(worldPos, Quaternion.identity);
                wallPresenters.Add(wallPresenter);
            }

            _tilePresenters = wallPresenters;
        }

        void DestroyWallPresenter()
        {
            foreach (var tilePresenter in _tilePresenters)
            {
                tilePresenter.DestroyPresenter();
            }

            _tilePresenters = new List<IPresenterMono>();
        }
    }
}