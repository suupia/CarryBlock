using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Spawners;
using Fusion;
using Projects.CarrySystem.Block.Scripts;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    public class TilePresenterBuilder
    {
        [Inject] NetworkRunner _runner;
        IEnumerable<TilePresenter_Net> _tilePresenters =  new List<TilePresenter_Net>();
        
        [Inject]
        public TilePresenterBuilder()
        {
        }
        
        public void Build(EntityGridMap map)
        {
            var tilePresenterSpawner = new TilePresenterSpawner(_runner);
            var tilePresenters = new List<TilePresenter_Net>();

            // 以前のTilePresenterを削除
            DestroyTilePresenter();
            
            // TilePresenterをスポーンさせる
            for (int i = 0; i < map.GetLength(); i++)
            {
                var girdPos = map.GetVectorFromIndex(i);
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
            _tilePresenters = new List<TilePresenter_Net>();
        }
        
         void AttachTilePresenter(IEnumerable<TilePresenter_Net> tilePresenters , EntityGridMap map)
        {
            for (int i = 0; i < tilePresenters.Count(); i++)
            {
                var tilePresenter = tilePresenters.ElementAt(i);

                // RegisterTilePresenter()の前なのでSetEntityActiveData()を実行する必要がある
                // Presenterの初期化処理みたいなもの
                var existGround = map.GetSingleEntity<Ground>(i) != null;
                var existRock = map.GetSingleEntity<Rock>(i) != null;
                var existBasicBlock = map.GetSingleEntity<BasicBlock>(i) != null;
                
                if(existRock) Debug.Log($"existGround: {existGround}, existRock: {existRock}, existBasicBlock: {existBasicBlock}");

                tilePresenter.SetInitAllEntityActiveData(map.GetAllEntityList(i)  );

                // mapにTilePresenterを登録
                map.RegisterTilePresenter(tilePresenter, i);
                
                
            }
        }
        
    }
}