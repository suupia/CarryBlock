﻿using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Spawners.Interfaces;
using Carry.CarrySystem.Spawners.Scripts;
using Fusion;
using UnityEngine;
using VContainer;
#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class RandomWallPresenterPlacerNet  : IPresenterPlacer
    {
        [Inject] NetworkRunner _runner;
        IEnumerable<IWallPresenter> _tilePresenters = new List<IWallPresenter>();

        readonly int _wallHorizontalNum = 3;
        readonly int _wallVerticalNum = 2;

        [Inject]
        public RandomWallPresenterPlacerNet()
        {
        }

        public void Place(EntityGridMap map)
        {
            var wallPresenterSpawners = new List<IWallPresenterSpawner>()
                { new WallPresenterNetSpawner(_runner), new WallPresenterNetSpawner1(_runner) };
            var wallPresenters = new List<IWallPresenter>();

            // 以前のWallPresenterを削除
            DestroyWallPresenter();

            // WallPresenterをスポーンさせる
            var expandedMap = new SquareGridMap(map.Width + 2 * _wallHorizontalNum, map.Height + 2 * _wallVerticalNum);
            for (int i = 0; i < expandedMap.Length; i++)
            {
                var gridPos = expandedMap.ToVector(i);
                var convertedGridPos = new Vector2Int(gridPos.x - _wallHorizontalNum, gridPos.y - _wallVerticalNum);
                if (map.IsInDataRangeArea(convertedGridPos)) continue;
                if (IsNotPlacingBlock(map, convertedGridPos)) continue;
                var worldPos = GridConverter.GridPositionToWorldPosition(convertedGridPos);
                var wallPresenterSpawner = DecideWallPresenterType(wallPresenterSpawners);
                var wallPresenter = wallPresenterSpawner.SpawnPrefab(worldPos, Quaternion.identity);
                wallPresenters.Add(wallPresenter);
                

            }
            
            _tilePresenters = wallPresenters;
        }

        IWallPresenterSpawner DecideWallPresenterType(List<IWallPresenterSpawner> wallPresenterSpawners)
        {
            var random = new System.Random();
            return wallPresenterSpawners[random.Next(2)];
        }

        void DestroyWallPresenter()
        {
            foreach (var tilePresenter in _tilePresenters)
            {
                tilePresenter.DestroyPresenter();;
            }

            _tilePresenters = new List<WallPresenterNet>();
        }
        
        bool IsNotPlacingBlock(EntityGridMap map, Vector2Int gridPos)
        {
            // 右端においては、ブロックがない場所には置かない
            if (gridPos.x >= map.Width)
            {
                if (map.GetSingleEntityList<IBlock>(new Vector2Int(gridPos.x, map.Width - 1)).Count == 0) return true;
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