﻿using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Spawners;
using Fusion;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    public class LobbyWallPresenterPlacer : IPresenterPlacer
    {
        [Inject] NetworkRunner _runner;
        IEnumerable<WallPresenterNet> _tilePresenters = new List<WallPresenterNet>();

        readonly int _wallHorizontalNum = 10;
        readonly int _wallVerticalNum = 10;

        [Inject]
        public LobbyWallPresenterPlacer()
        {
        }

        public void Place(EntityGridMap map)
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

            _tilePresenters = new List<WallPresenterNet>();
        }
    }
}