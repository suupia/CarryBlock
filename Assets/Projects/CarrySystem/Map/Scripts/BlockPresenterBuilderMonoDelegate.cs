﻿using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Spawners;
using Fusion;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    public class BlockPresenterBuilderMonoDelegate : IBlockPresenterBuilder
    {
                [Inject] NetworkRunner _runner;
        IEnumerable<BlockPresenterNet> _tilePresenters =  new List<BlockPresenterNet>();
        
        [Inject]
        public BlockPresenterBuilderMonoDelegate()
        {
        }
        
        public void Build(EntityGridMap map)
        {
            var tilePresenterSpawner = new BlockPresenterSpawner(_runner);
            var tilePresenters = new List<BlockPresenterNet>();

            // 以前のTilePresenterを削除
            DestroyTilePresenter();
            
            // TilePresenterをスポーンさせる
            for (int i = 0; i < map.GetLength(); i++)
            {
                var girdPos = map.ToVector(i);
                var worldPos = GridConverter.GridPositionToWorldPosition(girdPos);
                var tilePresenter = tilePresenterSpawner.SpawnPrefab(worldPos, Quaternion.identity);
                tilePresenters.Add(tilePresenter);
            }
            
            // TilePresenterをドメインのEntityGridMapに紐づける
            AttachTilePresenter(tilePresenters, map);

            _tilePresenters = tilePresenters;
        }
        
        void DestroyTilePresenter()
        {
            // マップの大きさが変わっても対応できるようにDestroyが必要
            // ToDo: マップの大きさを変えてテストをする 
            
            foreach (var tilePresenter in _tilePresenters)
            {
                _runner.Despawn(tilePresenter.Object);
            }
            _tilePresenters = new List<BlockPresenterNet>();
        }
        
         void AttachTilePresenter(IReadOnlyList<BlockPresenterNet> tilePresenters , EntityGridMap map)
        {
            for (int i = 0; i < tilePresenters.Count(); i++)
            {
                var tilePresenter = tilePresenters.ElementAt(i);

                // RegisterTilePresenter()の前なのでSetEntityActiveData()を実行する必要がある
                // Presenterの初期化処理みたいなもの
                var existGround = map.GetSingleEntity<Ground>(i) != null;
                var existRock = map.GetSingleEntity<UnmovableBlock>(i) != null;
                var existBasicBlock = map.GetSingleEntity<BasicBlock>(i) != null;
                
                if(existRock) Debug.Log($"existGround: {existGround}, existRock: {existRock}, existBasicBlock: {existBasicBlock}");

                tilePresenter.SetInitAllEntityActiveData(map.GetAllEntityList(i)  );

                // mapにTilePresenterを登録
                map.RegisterTilePresenter(tilePresenter, i);
                
                
            }
        }
        
    }
}