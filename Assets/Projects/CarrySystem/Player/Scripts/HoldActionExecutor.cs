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
        readonly IMapGetter _mapGetter;
        PlayerInfo _info = null!;
        EntityGridMap _map = null!;
        readonly PlayerHoldingObjectContainer _holdingObjectContainer;
        readonly PlayerNearCartHandlerNet _playerNearCartHandler;

        // Executor Component
        readonly HoldBlockComponent _holdBlockComponent;
        readonly HoldAidKitComponent _holdAidKitComponent;
        
        // Presenter
        IPlayerHoldablePresenter? _playerBlockPresenter;
        PlayerAidKitPresenterNet? _playerAidKitPresenter;
        IPlayerAnimatorPresenter? _playerAnimatorPresenter;

        IDisposable? _searchBlockDisposable;

        IBlockMonoDelegate? _searchedBlockMonoDelegate;
        IList<IBlock > _searchedBlocks = new List<IBlock>();

        AidKitRangeNet? _aidKitRangeNet;
        


        public HoldActionExecutor(
            PlayerHoldingObjectContainer holdingObjectContainer, 
            HoldBlockComponent holdBlockComponent,
            HoldAidKitComponent holdAidKitComponent,
            PlayerNearCartHandlerNet playerNearCartHandler,  // todo: 後で消す
            IMapGetter mapGetter)
        {
            _holdingObjectContainer = holdingObjectContainer;
            _holdBlockComponent = holdBlockComponent;
            _holdAidKitComponent = holdAidKitComponent;
            _playerNearCartHandler = playerNearCartHandler;
            _mapGetter = mapGetter;
        }

        public void Setup(PlayerInfo info)
        {
            _info = info; 
            _holdBlockComponent.Setup(info);
            _holdAidKitComponent.Setup(info);
            
        }

        public void Reset()
        {
            // reset holding block
            ResetHoldingBlock();
            
            // reset holding aid kit
            ResetHoldingAidKit();   
            
        }

        public void ResetHoldingBlock()
        {
           _holdBlockComponent.ResetHoldable();
        }
        
        void ResetHoldingAidKit()
        {
            _holdAidKitComponent.ResetHoldable();
        }
        
        public void HoldAction()
        {
            var transform = _info.PlayerObj.transform;
            var forwardGridPos = GetForwardGridPos(transform);
            
            Debug.Log($"IsHoldingBlock : {_holdingObjectContainer.IsHoldingBlock}");


            if (_holdingObjectContainer.IsHoldingBlock)
            {
                // trying to put down a block
                if(TryToPutDownBlock(forwardGridPos)) return;

            } else if (_holdingObjectContainer.IsHoldingAidKit)  // IsHoldingAidKit
            {
                // trying to use an aid kit
                
                // Even if character has an AidKit, player can overwrite with the block.
                if(TryToPickUpBlock(forwardGridPos)) return;

                if(TryToUseAidKit()) return;

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

        bool TryToPutDownBlock(Vector2Int targetPos)
        {
           return _holdBlockComponent.TryToUseHoldable(targetPos);
        }

        bool TryToUseAidKit()
        {
            return _holdAidKitComponent.TryToUseHoldable();
        }
        

        bool TryToPickUpBlock(Vector2Int forwardGridPos)
        {
            return _holdBlockComponent.TryToPickUpHoldable(forwardGridPos);

        }
        
        bool TryToPickUpAidKit()
        {
            return _holdAidKitComponent.TryToPickUpHoldable();
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
        
        public void SetPlayerBlockPresenter(IPlayerHoldablePresenter presenter)
        {
            _playerBlockPresenter = presenter;
             _holdBlockComponent.SetPlayerHoldablePresenter(presenter);
        }

        public void SetPlayerAidKitPresenter(PlayerAidKitPresenterNet presenter)
        {
            _playerAidKitPresenter = presenter;
            _holdAidKitComponent.SetPlayerHoldablePresenter(presenter);

        }
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _playerAnimatorPresenter = presenter;
            _holdBlockComponent.SetPlayerAnimatorPresenter(presenter);
            _holdAidKitComponent.SetPlayerAnimatorPresenter(presenter);
        }
        
    }
}