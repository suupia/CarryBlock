﻿using System;
using System.Collections.Generic;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Spawners.Interfaces;
using Carry.CarrySystem.Spawners.Scripts;
using Fusion;
using UnityEngine;
using VContainer;
#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class LocalWallPresenterPlacer
    {
        IEnumerable<WallPresenterLocal> _tilePresenters = new List<WallPresenterLocal>();
        
        [Inject]
        public LocalWallPresenterPlacer()
        {
        }

        public void Place(NetworkArray<NetworkBool> booleanMap, Int32 width, Int32 height, Int32 wallHorizontalNum, Int32 wallVerticalNum)
        {
            //var wallPresenterSpawner = new WallPresenterSpawner(_runner);
            var wallPresenterSpawners = new List<IWallPresenterLocalSpawner>()
                { new LocalWallPresenterSpawner(), new LocalWallPresenterSpawner1() };
            var wallPresenters = new List<WallPresenterLocal>();

            // 以前のWallPresenterを削除
            DestroyWallPresenter();

            // WallPresenterをスポーンさせる
            for (int i = 0; i < width * height; i++)  // 余剰分があるためboolMapのLengthではなくwidth * height
            {
                if (booleanMap[i])
                {
                    var gridPos = ToVector(i, width);
                    var convertedGridPos = new Vector2Int(gridPos.x - wallHorizontalNum, gridPos.y - wallVerticalNum);
                    var worldPos = GridConverter.GridPositionToWorldPosition(convertedGridPos);
                    var wallPresenterSpawner = DecideWallPresenterType(wallPresenterSpawners);
                    var wallPresenter = wallPresenterSpawner.SpawnPrefab(worldPos, Quaternion.identity);
                    wallPresenters.Add(wallPresenter);
                }
                
            }

            _tilePresenters = wallPresenters;
        }

        IWallPresenterLocalSpawner DecideWallPresenterType(List<IWallPresenterLocalSpawner> wallPresenterSpawners)
        {
            var random = new System.Random();
            return wallPresenterSpawners[random.Next(2)];
        }
        
        Vector2Int ToVector(int subscript, int width)
        {
            int x = subscript % width;
            int y = subscript / width;
            return new Vector2Int(x, y);
        }

        void DestroyWallPresenter()
        {

            foreach (var tilePresenter in _tilePresenters)
            {
                UnityEngine.Object.Destroy(tilePresenter);
            }

            _tilePresenters = new List<WallPresenterLocal>();
        }
    }
}