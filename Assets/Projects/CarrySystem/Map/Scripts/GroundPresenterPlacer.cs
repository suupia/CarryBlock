﻿#nullable enable
using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Spawners.Interfaces;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    public class GroundPresenterPlacer : IPresenterPlacer
    {
        readonly IGroundPresenterSpawner _groundPresenterSpawner;
        IEnumerable<IPresenterMono> _tilePresenters = new List<IPresenterMono>();

        int _groundHorizontalSurplus = 3;
        int _groundVerticalSurplus = 2;

        [Inject]
        public GroundPresenterPlacer(IGroundPresenterSpawner groundPresenterSpawner)
        {
            _groundPresenterSpawner = groundPresenterSpawner;
        }
        
        public void Place(EntityGridMap map)
        {
            var wallPresenters = new List<IPresenterMono>();

            // 以前のTilePresenterを削除
            DestroyWallPresenter();

            // GroundPresenterをスポーンさせる
            var expandedCoordinate = new SquareGridCoordinate(map.Width + 2 * _groundHorizontalSurplus, map.Height + 2 * _groundVerticalSurplus);
            for (int i = 0; i < expandedCoordinate.Length; i++)
            {
                var gridPos = expandedCoordinate.ToVector(i);
                var convertedGridPos = new Vector2Int(gridPos.x - _groundHorizontalSurplus, gridPos.y - _groundVerticalSurplus);
                var worldPos = GridConverter.GridPositionToWorldPosition(convertedGridPos);
                var wallPresenter = _groundPresenterSpawner.SpawnPrefab(worldPos, Quaternion.identity);
                wallPresenters.Add(wallPresenter);
            }

            _tilePresenters = wallPresenters;
        }
        
        public void SetSurPlus(int horizontalSurplus, int verticalSurplus)
        {
            _groundHorizontalSurplus = horizontalSurplus;
            _groundVerticalSurplus = verticalSurplus;
        }

        void DestroyWallPresenter()
        {
            foreach (var tilePresenter in _tilePresenters)
            {
                tilePresenter.DestroyPresenter();
            }

            _tilePresenters = new List<GroundPresenterLocal>();
        }
    }
}