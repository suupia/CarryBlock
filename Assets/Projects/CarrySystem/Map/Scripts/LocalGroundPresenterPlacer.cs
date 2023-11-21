#nullable enable
using System;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Spawners.Scripts;
using Fusion;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    public class LocalGroundPresenterPlacer
    {
        IEnumerable<IPresenterMono> _tilePresenters = new List<IPresenterMono>();

        readonly int _groundHorizontalNum = 3;
        readonly int _groundVerticalNum = 2;

        [Inject]
        public LocalGroundPresenterPlacer()
        {
        }

        public void Place(NetworkArray<NetworkBool> booleanMap, Int32 width, Int32 height )
        {
            var wallPresenterSpawner = new GroundPresenterLocalSpawner();
            var wallPresenters = new List<IPresenterMono>();

            // 以前のTilePresenterを削除
            DestroyWallPresenter();

            // GroundPresenterをスポーンさせる
            var expandedMap = new SquareGridMap(width+ 2 * _groundHorizontalNum, height + 2 * _groundVerticalNum);
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
                tilePresenter.DestroyPresenter();;
            }

            _tilePresenters = new List<GroundPresenterLocal>();
        }
    }
}