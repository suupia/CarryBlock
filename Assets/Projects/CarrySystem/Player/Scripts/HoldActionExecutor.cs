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
        EntityGridMap _map = null!;
        readonly PlayerHoldingObjectContainer _holdingObjectContainer;

        // Executor Component
        readonly HoldBlockComponent _holdBlockComponent;
        readonly HoldAidKitComponent _holdAidKitComponent;
        

        IDisposable? _searchBlockDisposable;

        IBlockMonoDelegate? _searchedBlockMonoDelegate;
        IList<IBlock > _searchedBlocks = new List<IBlock>();

        AidKitRangeNet? _aidKitRangeNet;
        


        public HoldActionExecutor(
            PlayerHoldingObjectContainer holdingObjectContainer, 
            HoldBlockComponent holdBlockComponent,
            HoldAidKitComponent holdAidKitComponent
            )
        {
            _holdingObjectContainer = holdingObjectContainer;
            _holdBlockComponent = holdBlockComponent;
            _holdAidKitComponent = holdAidKitComponent;
        }

        public void Setup(PlayerInfo info)
        {
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
           return _holdBlockComponent.TryToUseHoldable();
        }

        bool TryToUseAidKit()
        {
            return _holdAidKitComponent.TryToUseHoldable();
        }
        

        bool TryToPickUpBlock( )
        {
            return _holdBlockComponent.TryToPickUpHoldable();

        }
        
        bool TryToPickUpAidKit()
        {
            return _holdAidKitComponent.TryToPickUpHoldable();
        }
        
        // Presenter

        public void SetPlayerBlockPresenter(IPlayerHoldablePresenter presenter)
        {
             _holdBlockComponent.SetPlayerHoldablePresenter(presenter);
        }

        public void SetPlayerAidKitPresenter(PlayerAidKitPresenterNet presenter)
        {
            _holdAidKitComponent.SetPlayerHoldablePresenter(presenter);

        }
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _holdBlockComponent.SetPlayerAnimatorPresenter(presenter);
            _holdAidKitComponent.SetPlayerAnimatorPresenter(presenter);
        }
        
    }
}