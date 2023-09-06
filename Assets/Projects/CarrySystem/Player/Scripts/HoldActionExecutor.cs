﻿using System;
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
        IList<ICarriableBlock> _searchedBlocks = new List<ICarriableBlock>();

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
            var transform = _info.PlayerObj.transform;
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
                    block.PutDown(_info.PlayerController.GetCharacter);
                    // _map.AddEntity(forwardGridPos, block);
                    _map.GetSingleEntity<IBlockMonoDelegate>(forwardGridPos)?.AddBlock(block);
                    _playerPresenterContainer.PutDownBlock();
                }
            }
            else
            {
                var blockMonoDelegate = _searchedBlockMonoDelegate;  // フレームごとに判定しているためここでキャッシュする
                if(blockMonoDelegate?.Block == null)
                {
                    Debug.Log($"blockMonoDelegate.Block : null");
                    return;
                }
                
                // Debug
                Debug.Log($"before currentBlockMonos : {string.Join(",", _map.GetSingleEntityList<IBlockMonoDelegate>(forwardGridPos).Select(x => x.Block))}");

                var block = blockMonoDelegate.Block;
                if(!( block is ICarriableBlock carriableBlock)) return;
                if (carriableBlock.CanPickUp())
                {
                    Debug.Log($"remove currentBlockMonos");
                    carriableBlock.PickUp(_info.PlayerController.GetCharacter);
                    // _map.RemoveEntity(forwardGridPos,blockMonoDelegate);
                    _map.GetSingleEntity<IBlockMonoDelegate>(forwardGridPos)?.RemoveBlock(block);
                    _playerPresenterContainer.PickUpBlock(block);
                    _blockContainer.SetBlock(carriableBlock);
                }
                Debug.Log($"after currentBlockMonos : {string.Join(",", _map.GetSingleEntityList<IBlockMonoDelegate>(forwardGridPos).Select(x => x.Block))}");

            }
        }

        void SearchBlocks()
        {
            if (_info.PlayerObj == null) return; // EveryUpdateで呼ぶため、playerObjが破棄された後にも呼ばれる可能性がある
            var transform = _info.PlayerObj.transform;
            var forwardGridPos = GetForwardGridPos(transform);

            // 前方のMonoBlockDelegateを取得
            var blockMonoDelegate = _map.GetSingleEntity<IBlockMonoDelegate>(forwardGridPos);

            _searchedBlockMonoDelegate = blockMonoDelegate;
            
            if (blockMonoDelegate == null) return;

            // Debug.Log($"forwardGridPos: {forwardGridPos}, Blocks: {string.Join(",", blockMonoDelegate.Blocks)}");

            // _searchedBlockを更新
            _searchedBlocks = blockMonoDelegate.Blocks.OfType<ICarriableBlock>().ToList();

            // ハイライトの処理
            var block = blockMonoDelegate?.Block;
            if( block is not ICarriableBlock carriableBlock) return;
            if (carriableBlock.CanPickUp())
            {
                blockMonoDelegate?.Highlight(blockMonoDelegate.Block, _info.PlayerRef); // ハイライトの処理
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