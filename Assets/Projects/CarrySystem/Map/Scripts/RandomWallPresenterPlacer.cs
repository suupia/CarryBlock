﻿#nullable enable
using System.Collections.Generic;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Spawners.Interfaces;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    public class RandomWallPresenterPlacer  : IPresenterPlacer
    {
        readonly IWallPresenterSpawner _wallPresenterSpawner;
        IEnumerable<IPresenterMono> _tilePresenters = new List<IPresenterMono>();

        readonly int _wallHorizontalNum = 3;
        readonly int _wallVerticalNum = 2;

        [Inject]
        public RandomWallPresenterPlacer(IWallPresenterSpawner wallPresenterSpawner)
        {
            _wallPresenterSpawner = wallPresenterSpawner;
        }

        public void Place(EntityGridMap map)
        {
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
                if (IsNotPlacingBlock(map, convertedGridPos)) continue;
                var worldPos = GridConverter.GridPositionToWorldPosition(convertedGridPos);
                var wallPresenter = _wallPresenterSpawner.SpawnPrefab(worldPos, Quaternion.identity);
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

            _tilePresenters = new List<IPresenterMono>();
        }
        
        bool IsNotPlacingBlock(EntityGridMap map, Vector2Int gridPos)
        {
            // 右端においては、ブロックがない場所には置かない
            if (gridPos.x >= map.Width)
            {
                if (map.GetSingleTypeList<IBlock>(new Vector2Int(gridPos.x, map.Width - 1)).Count == 0) return true;
            }
            
            // 左端においては、真ん中から3マス分の範囲には置かない
            if (gridPos.x < 0)
            {
                if (map.Height / 2 - 1 <= gridPos.y && gridPos.y <= map.Height / 2 + 1) return true;
            }

            return false;
        }
    }
}