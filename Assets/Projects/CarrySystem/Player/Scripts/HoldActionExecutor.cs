using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Carry.CarrySystem.Player.Info;
using Cysharp.Threading.Tasks;
using Projects.CarrySystem.Block;
using Projects.CarrySystem.Block.Scripts;
using UniRx;

#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class HoldActionExecutor : IHoldActionExecutor
    {
        readonly IMapUpdater _mapUpdater;
        PlayerInfo _info = null!;
        EntityGridMap _map = null!;
        readonly PlayerBlockContainer _blockContainer;
        readonly IPlayerBlockPresenter _playerPresenterContainer;

        IDisposable? _searchBlockDisposable;

        IBlockMonoDelegate? _searchedBlockMonoDelegate;
        IList<IBlock> _searchedBlocks = new List<IBlock>();

        public HoldActionExecutor(
            PlayerBlockContainer blockContainer, 
            IPlayerBlockPresenter playerPresenterContainer,
            IMapUpdater mapUpdater)
        {
            _blockContainer = blockContainer;
            _playerPresenterContainer = playerPresenterContainer;    
            _mapUpdater = mapUpdater;
        }

        public void Setup(PlayerInfo info)
        {
            _info = info;
            _map = _mapUpdater.GetMap();

            _searchBlockDisposable?.Dispose();
            _searchBlockDisposable = Observable.EveryUpdate().Subscribe(_ =>
            {
                SearchBlocks();
            });
        }

        public void Reset()
        {
            var _ =  _blockContainer.PopBlock(); // Hold中のBlockがあれば取り出して削除
            _playerPresenterContainer.PutDownBlock();
            _map = _mapUpdater.GetMap(); // Resetが呼ばれる時点でMapが切り替わっている可能性があるため、再取得
        }
        public void HoldAction()
        {
            var transform = _info.playerObj.transform;
            var forwardGridPos = GetForwardGridPos(transform);
            
            Debug.Log($"IsHoldingBlock : {_blockContainer.IsHoldingBlock}");

            if (_blockContainer.IsHoldingBlock)
            {
                // マップの内部かどうかを判定
                if(!_map.IsInDataRangeArea(forwardGridPos))return;
                
                Debug.Log($"CanPutDown : {_blockContainer.CanPutDown(_searchedBlocks)}");
                if (_blockContainer.CanPutDown(_searchedBlocks))
                {
                    var block = _blockContainer.PopBlock();
                    if (block == null)
                    {
                        Debug.LogError($" _blockContainer.PopBlock() : null"); // IsHoldingBlockがtrueのときはnullにならないから呼ばれないはず
                        return;
                    }
                    block.PutDown(_info.playerController.GetCharacter);
                    // _map.AddEntity(forwardGridPos, block);
                    _map.GetSingleEntity<IBlockMonoDelegate>(forwardGridPos).AddBlock(block);
                    _playerPresenterContainer.PutDownBlock();
                }
            }
            else
            {
                var blockMonoDelegate = _searchedBlockMonoDelegate;  // フレームごとに判定しているためここでキャッシュする
                // if (blockMonoDelegate == null)
                // {
                //     Debug.Log($"_searchedBlockMonoDelegate : null");
                //     return;
                // }
                // else
                // {
                //     Debug.Log($"_searchedBlockMonoDelegate : {blockMonoDelegate.Block}");
                // }
                if(blockMonoDelegate.Block == null)
                {
                    Debug.Log($"blockMonoDelegate.Block : null");
                    return;
                }
                
                // Debug
                Debug.Log($"before currentBlockMonos : {string.Join(",", _map.GetSingleEntityList<IBlockMonoDelegate>(forwardGridPos).Select(x => x.Block))}");

                var block = blockMonoDelegate.Block;
                if (block.CanPickUp())
                {
                    Debug.Log($"remove currentBlockMonos");
                    block.PickUp(_info.playerController.GetCharacter);
                    // _map.RemoveEntity(forwardGridPos,blockMonoDelegate);
                    _map.GetSingleEntity<IBlockMonoDelegate>(forwardGridPos).RemoveBlock(block);
                    _playerPresenterContainer.PickUpBlock(block);
                    _blockContainer.SetBlock(block);
                }
                Debug.Log($"after currentBlockMonos : {string.Join(",", _map.GetSingleEntityList<IBlockMonoDelegate>(forwardGridPos).Select(x => x.Block))}");

            }
        }

        void SearchBlocks()
        {
            var transform = _info.playerObj.transform;
            var forwardGridPos = GetForwardGridPos(transform);

            // 前方のMonoBlockDelegateを取得
            var blockMonoDelegate = _map.GetSingleEntity<IBlockMonoDelegate>(forwardGridPos);


            _searchedBlockMonoDelegate = blockMonoDelegate;
            if (blockMonoDelegate != null)
            {
                Debug.Log($"forwardGridPos: {forwardGridPos}, Blocks: {string.Join(",", blockMonoDelegate.Blocks)}");

                _searchedBlocks = blockMonoDelegate.Blocks;
                blockMonoDelegate.Highlight(blockMonoDelegate.Block);  // ハイライトの処理

            }
            

        }


        Vector2Int GetForwardGridPos(Transform transform)
        {
            var gridPos = GridConverter.WorldPositionToGridPosition(transform.position);
            var forward = transform.forward;
            var direction = new Vector2(forward.x, forward.z);
            var gridDirection = GridConverter.WorldDirectionToGridDirection(direction);
            return gridPos + gridDirection;
        }
        
    }
}