using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Spawners;
using Fusion;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    public class TilePresenterBuilder
    {
        [Inject] NetworkRunner _runner;
        readonly TilePresenterAttacher _tilePresenterAttacher;
        IEnumerable<TilePresenter_Net> _tilePresenters =  new List<TilePresenter_Net>();
        
        [Inject]
        public TilePresenterBuilder(TilePresenterAttacher tilePresenterAttacher)
        {
            _tilePresenterAttacher = tilePresenterAttacher;
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
            _tilePresenterAttacher.AttachTilePresenter(tilePresenters, map);

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
        
    }
}