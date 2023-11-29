using System;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.CarriableBlock.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UniRx;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class HoldBlockActionComponent : IHoldableActionComponent
    {       
        PlayerInfo _info = null!;
        AidKitRangeNet? _aidKitRangeNet;
        IList<IBlock > _searchedBlocks = new List<IBlock>();
        IDisposable? _searchBlockDisposable;

        readonly IMapGetter _mapGetter;
        readonly PlayerHoldingObjectContainer _holdingObjectContainer;

        // Presenter
        IPlayerHoldablePresenter? _playerBlockPresenter;
        IPlayerHoldablePresenter? _playerAidKitPresenter;  // todo : この依存どうにかなる？
        IPlayerAnimatorPresenter? _playerAnimatorPresenter;

        public HoldBlockActionComponent(
            PlayerHoldingObjectContainer holdingObjectContainer,
            IMapGetter mapGetter)
        {
            _holdingObjectContainer = holdingObjectContainer;
            _mapGetter = mapGetter;
        }
        
        public void Setup(PlayerInfo info)
        {
            _info = info;

            _searchBlockDisposable?.Dispose();
            _searchBlockDisposable = Observable.EveryUpdate().Subscribe(_ =>
            {
                SearchBlocks();
            });
        }

        public void ResetHoldable()
        {
            var _ =  _holdingObjectContainer.PopBlock(); // Hold中のBlockがあれば取り出して削除
            _playerBlockPresenter?.DisableHoldableView();
            _playerAnimatorPresenter?.PutDownBlock();

        }
        
        public bool TryToPickUpHoldable()
        { 
            var transform = _info.PlayerObj.transform;
            var forwardGridPos = GetForwardGridPos(transform);
            var searchedBlock = _searchedBlocks.FirstOrDefault();
            if(searchedBlock == null)
            {
                Debug.Log($"_preSearchedBlock : null");
                return false;
            }
                
            // Debug
            Debug.Log($"before IBlockList : {string.Join(",", _mapGetter.GetMap().GetSingleEntityList<IBlock>(forwardGridPos))}");
            
            if(!( _searchedBlocks.FirstOrDefault() is ICarriableBlock carriableBlock)) return false;
            if (carriableBlock.CanPickUp())
            {
                Debug.Log($"remove currentBlockMonos");
                carriableBlock.PickUp(_info.PlayerController.GetMoveExecutorSwitcher,_info.PlayerController.GetHoldActionExecutor);
                // _map.RemoveEntity(forwardGridPos,blockMonoDelegate);
                _mapGetter.GetMap().GetSingleEntity<BlockMonoDelegate>(forwardGridPos)?.RemoveBlock(searchedBlock);
                if (carriableBlock is IHoldable holdable)
                {
                    _playerBlockPresenter?.EnableHoldableView(holdable);
                }
                else
                {
                    Debug.LogError($"carriableBlock is not IHoldable. carriableBlock : {carriableBlock}");
                }
                _playerAnimatorPresenter?.PickUpBlock(searchedBlock);
                _holdingObjectContainer.SetBlock(carriableBlock);
            }
            Debug.Log($"after IBlockList : {string.Join(",", _mapGetter.GetMap().GetSingleEntityList<IBlock>(forwardGridPos))}");
            
            // もしAidKitを持っていたらブロックで上書きする
            if (_holdingObjectContainer.IsHoldingAidKit)
            {
                _holdingObjectContainer.PopAidKit();
                if(_playerAidKitPresenter != null) _playerAidKitPresenter.DisableHoldableView();
            }

            return true; // done picking up
        }


        public bool TryToUseHoldable()
        {
            var transform = _info.PlayerObj.transform;
            var forwardGridPos = GetForwardGridPos(transform);
            
            // マップの内部かどうかを判定
            if(!_mapGetter.GetMap().IsInDataRangeArea(forwardGridPos))return false;
                
            // if there is a non carriable block in front of a player, do nothing
            if (_searchedBlocks.Any(x => !(x is ICarriableBlock)))
            {
                Debug.Log($"There is a non carriable block in front of a player");
                return false;
            }
            var carriableBlocks = _searchedBlocks.OfType<ICarriableBlock>().ToList();
                
            Debug.Log($"T : {_holdingObjectContainer.CanPutDown(carriableBlocks)}");
            if (_holdingObjectContainer.CanPutDown(carriableBlocks))
            {
                var block = _holdingObjectContainer.PopBlock();
                if (block == null)
                {
                    Debug.LogError($" _blockContainer.PopBlock() : null"); // IsHoldingBlockがtrueのときはnullにならないから呼ばれないはず
                    return false;
                }
                block.PutDown(_info.PlayerController.GetMoveExecutorSwitcher);
                // _map.AddEntity(forwardGridPos, block);
                _mapGetter.GetMap().GetSingleEntity<BlockMonoDelegate>(forwardGridPos)?.AddBlock(block,forwardGridPos);
                _playerBlockPresenter?.DisableHoldableView();
                _playerAnimatorPresenter?.PutDownBlock();
            }

            return true;
        }
        
        void SearchBlocks()
        {
            if (_info.PlayerObj == null) return; // EveryUpdateで呼ぶため、playerObjが破棄された後にも呼ばれる可能性がある
            var transform = _info.PlayerObj.transform;
            var forwardGridPos = GetForwardGridPos(transform);

            // 前方のMonoBlockDelegateを取得
            var blockMonoDelegate = _mapGetter.GetMap().GetSingleEntity<BlockMonoDelegate>(forwardGridPos);
            
            // _searchedBlockを更新
            _searchedBlocks = _mapGetter.GetMap().GetSingleEntityList<IBlock>(forwardGridPos).ToList();
            var searchedBlock = _searchedBlocks.FirstOrDefault();
            
            if(searchedBlock == null) return;

            // ハイライトの処理
            if( searchedBlock is not ICarriableBlock carriableBlock) return;
            if (_holdingObjectContainer.IsHoldingBlock)
            {
                var carriableBlocks = _searchedBlocks.OfType<ICarriableBlock>().ToList();
                if (!carriableBlock.CanPutDown(carriableBlocks)) return;
            }
            else
            {
                if(!carriableBlock.CanPickUp())return;
            }
            blockMonoDelegate?.Highlight(searchedBlock, _info.PlayerRef); // ハイライトの処理

        }
        
        Vector2Int GetForwardGridPos(Transform transform)
        {
            var gridPos = GridConverter.WorldPositionToGridPosition(transform.position);
            var forward = transform.forward;
            var direction = new Vector2(forward.x, forward.z);
            var gridDirection = GridConverter.WorldDirectionToGridDirection(direction);
            return gridPos + gridDirection;
        }
        
        // View
        public void SetPlayerHoldablePresenter(IPlayerHoldablePresenter presenter)
        {
            _playerBlockPresenter = presenter;
        }
        
        // Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _playerAnimatorPresenter = presenter;

        }

    }
}