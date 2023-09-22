using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Spawners;
using Fusion;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    public class LobbyGroundPresenterPlacer : IPresenterPlacer
    {
        [Inject] NetworkRunner _runner;
        IEnumerable<GroundPresenterNet> _tilePresenters = new List<GroundPresenterNet>();

        readonly int _groundHorizontalNum = 10;
        readonly int _groundVerticalNum = 10;

        [Inject]
        public LobbyGroundPresenterPlacer()
        {
        }

        public void Place(EntityGridMap map)
        {
            var wallPresenterSpawner = new GroundPresenterSpawner(_runner);
            var wallPresenters = new List<GroundPresenterNet>();

            // 以前のTilePresenterを削除
            DestroyWallPresenter();

            // GroundPresenterをスポーンさせる
            var expandedMap = new SquareGridMap(map.Width + 2 * _groundHorizontalNum, map.Height + 2 * _groundVerticalNum);
            for (int i = 0; i < expandedMap.Length; i++)
            {
                var gridPos = expandedMap.ToVector(i);
                var convertedGridPos = new Vector2Int(gridPos.x - _groundHorizontalNum, gridPos.y - _groundVerticalNum);
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
                _runner.Despawn(tilePresenter.Object);
            }

            _tilePresenters = new List<GroundPresenterNet>();
        }
    }
}