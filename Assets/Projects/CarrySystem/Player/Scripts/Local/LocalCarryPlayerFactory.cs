﻿using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using VContainer;
#nullable enable

namespace Projects.CarrySystem.Player.Scripts.Local
{
    public class LocalCarryPlayerFactory : ICarryPlayerFactory
    {
        readonly IMapGetter _mapGetter;
        readonly HoldingBlockObserver  _holdingBlockObserver;

        [Inject]
        public LocalCarryPlayerFactory(
            IMapGetter mapGetter ,
            HoldingBlockObserver holdingBlockObserver
        )
        {
            _mapGetter = mapGetter;
            _holdingBlockObserver = holdingBlockObserver;
        }
        
        public  Character CreateCharacter()
        {
            // PlayerHolderObjectContainer
            var blockContainer = new PlayerHoldingObjectContainer();
            
            // IMoveExecutorSwitcher
            var moveExecutorSwitcher = new MoveExecutorSwitcher();
            
            // IHoldActionExecutor
            _holdingBlockObserver.RegisterHoldAction(blockContainer);
            var holdBlockExe = new HoldBlockActionComponent(blockContainer,_mapGetter);
            var holdActionExecutor =new HoldActionExecutor(blockContainer );
            holdActionExecutor.RegisterHoldBlockActionComponent(holdBlockExe);
            
            // IOnDamageExecutor
            var onDamageExecutor = new OnDamageExecutor(moveExecutorSwitcher, new PlayerCharacterTransporter());  //todo: new はエラー回避のため一時的に使用
            var dashExecutor = new DashExecutor(moveExecutorSwitcher, onDamageExecutor);
            
            // IPassActionExecutor
            var passBlockMoveExe = new PassWaitExecutor();
            var passActionExecutor = new PassActionExecutor(blockContainer, passBlockMoveExe); 
            
            var character = new Character(
                moveExecutorSwitcher,
                holdActionExecutor,
                dashExecutor,
                passActionExecutor,
                onDamageExecutor,
                blockContainer
            );
            
            return character;
        }
    }
}