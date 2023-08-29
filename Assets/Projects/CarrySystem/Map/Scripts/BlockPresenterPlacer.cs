using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Spawners;
using Fusion;
using Projects.CarrySystem.Block.Scripts;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    public class BlockPresenterPlacer : IBlockPresenterPlacer
    {
        [Inject] NetworkRunner _runner;
        IEnumerable<BlockPresenterNet> _blockPresenters =  new List<BlockPresenterNet>();
        
        [Inject]
        public BlockPresenterPlacer()
        {
        }
        
        public void Place(EntityGridMap map)
        {
            var blockPresenterSpawner = new BlockPresenterSpawner(_runner);
            var blockPresenterNets = new List<BlockPresenterNet>();

            // 以前のTilePresenterを削除
            DestroyTilePresenter();
            
            // BlockPresenterをスポーンさせる
            for (int i = 0; i < map.GetLength(); i++)
            {
                var girdPos = map.ToVector(i);
                var worldPos = GridConverter.GridPositionToWorldPosition(girdPos);
                var blockPresenterNet = blockPresenterSpawner.SpawnPrefab(worldPos, Quaternion.identity);
                // var blockControllers = blockPresenterNet.GetComponentsInChildren<BlockControllerNet>();
                // foreach (var blockController in blockControllers)
                // {
                //     blockController.Set
                // }
                blockPresenterNets.Add(blockPresenterNet);
            }
            
            // BlockPresenterをドメインのEntityGridMapに紐づける
            AttachTilePresenter(blockPresenterNets, map);

            _blockPresenters = blockPresenterNets;
        }
        
        void DestroyTilePresenter()
        {
            // マップの大きさが変わっても対応できるようにDestroyが必要
            // ToDo: マップの大きさを変えてテストをする 
            
            foreach (var blockPresenterNet in _blockPresenters)
            {
                _runner.Despawn(blockPresenterNet.Object);
            }
            _blockPresenters = new List<BlockPresenterNet>();
        }
        
         void AttachTilePresenter(IReadOnlyList<BlockPresenterNet> blockPresenterNets , EntityGridMap map)
        {
            for (int i = 0; i < blockPresenterNets.Count(); i++)
            {
                var blockPresenterNet = blockPresenterNets.ElementAt(i);

                // RegisterTilePresenter()の前なのでSetEntityActiveData()を実行する必要がある
                // Presenterの初期化処理みたいなもの
                var existGround = map.GetSingleEntity<Ground>(i) != null;
                var existUnmovableBlock = map.GetSingleEntity<UnmovableBlock>(i) != null;
                var existBasicBlock = map.GetSingleEntity<BasicBlock>(i) != null;
                
                // Debug.Log($"existGround: {existGround}, existUnmovableBlock: {existUnmovableBlock}, existBasicBlock: {existBasicBlock}");

                blockPresenterNet.SetInitAllEntityActiveData(map.GetAllEntityList(i)  );

                // mapにTilePresenterを登録
                map.RegisterTilePresenter(blockPresenterNet, i);
                
                
            }
        }
        
    }
}