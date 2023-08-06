﻿using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using UnityEngine;
using Carry.CarrySystem.Player.Info;
using Projects.CarrySystem.Block.Interfaces;

#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class Character : ICharacter
    {
        readonly IMoveExecutor _moveExecutor;
        readonly IHoldActionExecutor _holdActionExecutor;
        readonly IPassActionExecutor _passActionExecutor;
        readonly PlayerBlockContainer _blockContainer;
        
        public Character(
            IMoveExecutor moveExecutor, 
            IHoldActionExecutor holdActionExecutor,
            IPassActionExecutor passActionExecutor,
            PlayerBlockContainer blockContainer)
        {
            _moveExecutor = moveExecutor;
            _holdActionExecutor = holdActionExecutor;
            _passActionExecutor = passActionExecutor;
            _blockContainer = blockContainer;
        }

        public void Reset()
        {
            _holdActionExecutor.Reset();
            _passActionExecutor.Reset();
        }

        public void Setup(PlayerInfo info)
        {
            _moveExecutor.Setup(info);
            _holdActionExecutor.Setup(info);
            _passActionExecutor.Setup(info);
            info.playerRb.useGravity = true;
        }

        public void Move(Vector3 direction)
        {
            _moveExecutor.Move(direction);
        }
        
        public void SetHoldPresenter(IPlayerBlockPresenter presenter)
        {
            _blockContainer.SetHoldPresenter(presenter);
        }

        public void HoldAction()
        {
            _holdActionExecutor.HoldAction();
        }
        
        // PassAction
        public void PassAction() => _passActionExecutor.PassAction();
        public bool CanReceivePass() => _passActionExecutor.CanReceivePass();
        public void ReceivePass(IBlock block) => _passActionExecutor.ReceivePass(block);
    }
}