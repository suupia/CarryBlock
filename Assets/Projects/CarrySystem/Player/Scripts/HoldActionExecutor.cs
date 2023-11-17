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
        readonly PlayerHoldingObjectContainer _holdingObjectContainer;

        // Executor Component
        readonly HoldBlockActionComponent _holdBlockActionComponent;
        readonly HoldAidKitActionComponent _holdAidKitActionComponent;

        public HoldActionExecutor(
            PlayerHoldingObjectContainer holdingObjectContainer, 
            HoldBlockActionComponent holdBlockActionComponent,
            HoldAidKitActionComponent holdAidKitActionComponent
            )
        {
            _holdingObjectContainer = holdingObjectContainer;
            _holdBlockActionComponent = holdBlockActionComponent;
            _holdAidKitActionComponent = holdAidKitActionComponent;
        }

        public void Setup(PlayerInfo info)
        {
            _holdBlockActionComponent.Setup(info);
            _holdAidKitActionComponent.Setup(info);
            
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
           _holdBlockActionComponent.ResetHoldable();
        }
        
        void ResetHoldingAidKit()
        {
            _holdAidKitActionComponent.ResetHoldable();
        }
        
        public void HoldAction()
        {
            Debug.Log($"IsHoldingBlock : {_holdingObjectContainer.IsHoldingBlock}");
            
            if (_holdingObjectContainer.IsHoldingBlock)
            {
                // trying to put down a block
                if(TryToPutDownBlock()) return;

            } else if (_holdingObjectContainer.IsHoldingAidKit)  // IsHoldingAidKit
            {
                // trying to use an aid kit
                
                // Even if character has an AidKit, player can overwrite with the block.
                if(TryToPickUpBlock()) return;

                if(TryToUseAidKit()) return;

            }
            else
            {
                // try to pick up a block or an aid kit
                // judge priority is block > aid kit > nothing
                
                if(TryToPickUpBlock()) return; 

                if(TryToPickUpAidKit()) return;
                
                // nothing is in front of the player

            }
            
        }

        bool TryToPutDownBlock( )
        {
           return _holdBlockActionComponent.TryToUseHoldable();
        }

        bool TryToUseAidKit()
        {
            return _holdAidKitActionComponent.TryToUseHoldable();
        }
        

        bool TryToPickUpBlock( )
        {
            return _holdBlockActionComponent.TryToPickUpHoldable();

        }
        
        bool TryToPickUpAidKit()
        {
            return _holdAidKitActionComponent.TryToPickUpHoldable();
        }
        
        // Presenter

        public void SetPlayerBlockPresenter(IPlayerHoldablePresenter presenter)
        {
             _holdBlockActionComponent.SetPlayerHoldablePresenter(presenter);
        }

        public void SetPlayerAidKitPresenter(PlayerAidKitPresenterNet presenter)
        {
            _holdAidKitActionComponent.SetPlayerHoldablePresenter(presenter);

        }
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _holdBlockActionComponent.SetPlayerAnimatorPresenter(presenter);
            _holdAidKitActionComponent.SetPlayerAnimatorPresenter(presenter);
        }
        
    }
}