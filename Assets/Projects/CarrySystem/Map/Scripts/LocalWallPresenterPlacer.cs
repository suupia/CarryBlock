using System;
using System.Collections.Generic;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Spawners;
using Fusion;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using VContainer;
#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class LocalWallPresenterPlacer
    {
        IEnumerable<WallPresenterMono> _tilePresenters = new List<WallPresenterMono>();

        readonly int _wallHorizontalNum = 3;
        readonly int _wallVerticalNum = 2;

        [Inject]
        public LocalWallPresenterPlacer()
        {
        }

        public void Place(NetworkArray<NetworkBool> booleanMap, Int32 width, Int32 height)
        {
            //var wallPresenterSpawner = new WallPresenterSpawner(_runner);
            var wallPresenterSpawners = new List<IWallPresenterMonoSpawner>()
                { new LocalWallPresenterSpawner(), new LocalWallPresenterSpawner1() };
            var wallPresenters = new List<WallPresenterMono>();

            // 以前のWallPresenterを削除
            DestroyWallPresenter();

            // WallPresenterをスポーンさせる
            for (int i = 0; i < booleanMap.Length; i++)
            {
                if (booleanMap[i])
                {
                    var wallPresenterSpawner = DecideWallPresenterType(wallPresenterSpawners);
                    var wallPresenter = wallPresenterSpawner.SpawnPrefab(Vector3.zero, Quaternion.identity);
                    wallPresenters.Add(wallPresenter);
                }
            
            }
            
            // var expandedMap = new SquareGridMap(width+ 2 * _wallHorizontalNum, height + 2 * _wallVerticalNum);
            // for (int i = 0; i < expandedMap.Length; i++)
            // {
            //     var gridPos = expandedMap.ToVector(i);
            //     var convertedGridPos = new Vector2Int(gridPos.x - _wallHorizontalNum, gridPos.y - _wallVerticalNum);
            //     if (map.IsInDataRangeArea(convertedGridPos)) continue;
            //     if (IsNotPlacingBlock(map, convertedGridPos)) continue;
            //     var worldPos = GridConverter.GridPositionToWorldPosition(convertedGridPos);
            //     var wallPresenterSpawner = DecideWallPresenterType(wallPresenterSpawners);
            //     var wallPresenter = wallPresenterSpawner.SpawnPrefab(worldPos, Quaternion.identity);
            //     wallPresenters.Add(wallPresenter);
            //     
            //
            // }
            
            _tilePresenters = wallPresenters;
        }

        IWallPresenterMonoSpawner DecideWallPresenterType(List<IWallPresenterMonoSpawner> wallPresenterSpawners)
        {
            var random = new System.Random();
            return wallPresenterSpawners[random.Next(2)];
        }

        void DestroyWallPresenter()
        {
            // マップの大きさが変わっても対応できるようにDestroyが必要
            // ToDo: マップの大きさを変えてテストをする 

            foreach (var tilePresenter in _tilePresenters)
            {
                UnityEngine.Object.Destroy(tilePresenter);
            }

            _tilePresenters = new List<WallPresenterMono>();
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