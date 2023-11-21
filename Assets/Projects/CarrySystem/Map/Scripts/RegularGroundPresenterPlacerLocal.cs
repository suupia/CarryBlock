#nullable enable
using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Spawners.Scripts;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    public class RegularGroundPresenterPlacerLocal : IPresenterPlacer
    {
        IEnumerable<IGroundPresenter> _tilePresenters = new List<IGroundPresenter>();

        readonly int _groundHorizontalNum = 3;
        readonly int _groundVerticalNum = 2;

        [Inject]
        public RegularGroundPresenterPlacerLocal()
        {
        }

        public void Place(EntityGridMap map)
        {
            var wallPresenterSpawner = new GroundPresenterLocalSpawner();
            var wallPresenters = new List<IGroundPresenter>();

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
                tilePresenter.DestroyPresenter();
            }

            _tilePresenters = new List<GroundPresenterLocal>();
        }
    }
}