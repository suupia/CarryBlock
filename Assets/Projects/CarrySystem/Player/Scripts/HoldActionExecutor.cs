using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.CarriableBlock.Interfaces;
using Carry.CarrySystem.Cart.Scripts;
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
        readonly IMapSwitcher _mapSwitcher;
        PlayerInfo _info = null!;
        EntityGridMap _map = null!;
        readonly PlayerHoldingObjectContainer _holdingObjectContainer;
        readonly PlayerNearCartHandlerNet _playerNearCartHandler;
        
        // Presenter
        IPlayerBlockPresenter? _playerBlockPresenter;
        PlayerAidKitPresenterNet? _playerAidKitPresenter;
        IPlayerAnimatorPresenter? _playerAnimatorPresenter;

        IDisposable? _searchBlockDisposable;

        IBlockMonoDelegate? _searchedBlockMonoDelegate;
        IList<IBlock > _searchedBlocks = new List<IBlock>();

        AidKitRangeNet? _aidKitRangeNet;

        public HoldActionExecutor(
            PlayerHoldingObjectContainer holdingObjectContainer, 
            PlayerNearCartHandlerNet playerNearCartHandler,
            IMapSwitcher mapSwitcher)
        {
            _holdingObjectContainer = holdingObjectContainer;
            _playerNearCartHandler = playerNearCartHandler;
            _mapSwitcher = mapSwitcher;
        }

        public void Setup(PlayerInfo info)
        {
            _info = info;
            _map = _mapSwitcher.GetMap();

            _searchBlockDisposable?.Dispose();
            _searchBlockDisposable = Observable.EveryUpdate().Subscribe(_ =>
            {
                SearchBlocks();
            });
        }

        public void Reset()
        {
            // reset holding block
            var _ =  _holdingObjectContainer.PopBlock(); // Hold中のBlockがあれば取り出して削除
            _playerBlockPresenter?.PutDownBlock();
            _playerAnimatorPresenter?.PutDownBlock();
            
            // reset holding aid kit
            _holdingObjectContainer.PopAidKit();
            if (_playerAidKitPresenter != null) _playerAidKitPresenter.DisableAidKit();
            
            _map = _mapSwitcher.GetMap(); // Resetが呼ばれる時点でMapが切り替わっている可能性があるため、再取得
        }

        /// <summary>
        /// This method is called when the other class wants to put down a block.
        /// </summary>
        public void PutDownBlock()
        {
            _playerBlockPresenter?.PutDownBlock();
            _playerAnimatorPresenter?.PutDownBlock();
        }
        public void HoldAction()
        {
            var transform = _info.PlayerObj.transform;
            var forwardGridPos = GetForwardGridPos(transform);
            
            Debug.Log($"IsHoldingBlock : {_holdingObjectContainer.IsHoldingBlock}");


            if (_holdingObjectContainer.IsHoldingBlock)
            {
                // trying to put down a block
                // マップの内部かどうかを判定
                if(!_map.IsInDataRangeArea(forwardGridPos))return;
                
                // if there is a non carriable block in front of a player, do nothing
                if (_searchedBlocks.Any(x => !(x is ICarriableBlock)))
                {
                    Debug.Log($"There is a non carriable block in front of a player");
                    return;
                }
                var carriableBlocks = _searchedBlocks.OfType<ICarriableBlock>().ToList();
                
                Debug.Log($"CanPutDown : {_holdingObjectContainer.CanPutDown(carriableBlocks)}");
                if (_holdingObjectContainer.CanPutDown(carriableBlocks))
                {
                    var block = _holdingObjectContainer.PopBlock();
                    if (block == null)
                    {
                        Debug.LogError($" _blockContainer.PopBlock() : null"); // IsHoldingBlockがtrueのときはnullにならないから呼ばれないはず
                        return;
                    }
                    block.PutDown(_info.PlayerController.GetMoveExecutorSwitcher);
                    // _map.AddEntity(forwardGridPos, block);
                    _map.GetSingleEntity<IBlockMonoDelegate>(forwardGridPos)?.AddBlock(block);
                    _playerBlockPresenter?.PutDownBlock();
                    _playerAnimatorPresenter?.PutDownBlock();
                }
                
            } else if (_holdingObjectContainer.IsHoldingAidKit)  // IsHoldingAidKit
            {
                // trying to use an aid kit
                
                // Even if character has an AidKit, player can overwrite with the block.
                if(TryToPickUpBlock(forwardGridPos)) return;

                // もし倒れているキャラが近くにいれば、AidKitを使う
                // 1. PlayerControllerを取得
                // 2. ICharacterを取得
                // 3. IsFaintedで判定
                
                if(_aidKitRangeNet == null) _aidKitRangeNet = _info.PlayerObj.GetComponentInChildren<AidKitRangeNet>();
                
                if(_aidKitRangeNet.DetectedTarget() is {} target)
                {
                    var targetPlayerController = target.GetComponent<CarryPlayerControllerNet>();
                    if (targetPlayerController == null)
                    {
                        Debug.LogError($"{target.name} には CarryPlayerControllerNet がアタッチされていません");
                        return;
                    }
                    if (!targetPlayerController.GetOnDamageExecutor.IsFainted) return;
                    Debug.Log($"Use AidKit");
                    _holdingObjectContainer.PopAidKit();
                    if(_playerAidKitPresenter != null) _playerAidKitPresenter.UseAidKit();
                    targetPlayerController.GetOnDamageExecutor.OnRevive();
                }
                else
                {
                    // Do nothing
                }

            }
            else
            {
                // try to pick up a block or an aid kit
                // judge priority is block > aid kit > nothing
                
                if(TryToPickUpBlock(forwardGridPos)) return; 

                if(TryToPickUpAidKit()) return;
                
                // nothing is in front of the player

            }
            
        }
        

        bool TryToPickUpBlock(Vector2Int forwardGridPos)
        {
            var blockMonoDelegate = _searchedBlockMonoDelegate;  // フレームごとに判定しているためここでキャッシュする
            if(blockMonoDelegate?.Block == null)
            {
                Debug.Log($"blockMonoDelegate.Block : null");
                return false;
            }
                
            // Debug
            Debug.Log($"before currentBlockMonos : {string.Join(",", _map.GetSingleEntityList<IBlockMonoDelegate>(forwardGridPos).Select(x => x.Block))}");

            var block = blockMonoDelegate.Block;
            if(!( block is ICarriableBlock carriableBlock)) return false;
            if (carriableBlock.CanPickUp())
            {
                Debug.Log($"remove currentBlockMonos");
                carriableBlock.PickUp(_info.PlayerController.GetMoveExecutorSwitcher, _info.PlayerController.GetPlayerHoldingObjectContainer,_info.PlayerController.GetHoldActionExecutor);
                // _map.RemoveEntity(forwardGridPos,blockMonoDelegate);
                _map.GetSingleEntity<IBlockMonoDelegate>(forwardGridPos)?.RemoveBlock(block);
                _playerBlockPresenter?.PickUpBlock(block);
                _playerAnimatorPresenter?.PickUpBlock(block);
                _holdingObjectContainer.SetBlock(carriableBlock);
            }
            Debug.Log($"after currentBlockMonos : {string.Join(",", _map.GetSingleEntityList<IBlockMonoDelegate>(forwardGridPos).Select(x => x.Block))}");
            
            // もしAidKitを持っていたらブロックで上書きする
            if (_holdingObjectContainer.IsHoldingAidKit)
            {
                _holdingObjectContainer.PopAidKit();
                if(_playerAidKitPresenter != null) _playerAidKitPresenter.DisableAidKit();
            }

            return true; // done picking up

        }
        
        bool TryToPickUpAidKit()
        {
            // もしカートの近くにいれば、AidKitを拾う
            if (_playerNearCartHandler.IsNearCart(_info.PlayerObj))
            {
                if(_holdingObjectContainer.IsHoldingAidKit) return false;
                
                // 拾う処理
                Debug.Log($"PickUpAidKit");
                _holdingObjectContainer.SetAidKit();
                if(_playerAidKitPresenter != null) _playerAidKitPresenter.PickUpAidKit();
            }
            return false;
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
            _searchedBlocks = blockMonoDelegate.Blocks.OfType<IBlock>().ToList();

            // ハイライトの処理
            var block = blockMonoDelegate?.Block;
            if( block is not ICarriableBlock carriableBlock) return;
            if (_holdingObjectContainer.IsHoldingBlock)
            {
                var carriableBlocks = _searchedBlocks.OfType<ICarriableBlock>().ToList();
                if (!carriableBlock.CanPutDown(carriableBlocks)) return;
            }
            else
            {
                if(!carriableBlock.CanPickUp())return;
            }
            blockMonoDelegate?.Highlight(blockMonoDelegate.Block, _info.PlayerRef); // ハイライトの処理

        }

        // Presenter
        Vector2Int GetForwardGridPos(Transform transform)
        {
            var gridPos = GridConverter.WorldPositionToGridPosition(transform.position);
            var forward = transform.forward;
            var direction = new Vector2(forward.x, forward.z);
            var gridDirection = GridConverter.WorldDirectionToGridDirection(direction);
            return gridPos + gridDirection;
        }
        
        public void SetPlayerBlockPresenter(IPlayerBlockPresenter presenter)
        {
            _playerBlockPresenter = presenter;
            Debug.Log($"_playerBlockPresenter : {presenter}");
        }

        public void SetPlayerAidKitPresenter(PlayerAidKitPresenterNet presenter)
        {
            _playerAidKitPresenter = presenter;
        }
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _playerAnimatorPresenter = presenter;
        }
        
    }
}