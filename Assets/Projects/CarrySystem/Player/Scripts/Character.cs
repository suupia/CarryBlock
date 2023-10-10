using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.CarriableBlock.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using UnityEngine;
using Carry.CarrySystem.Player.Info;

#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class Character : ICharacter
    {
        public PlayerHoldingObjectContainer PlayerHoldingObjectContainer { get; }

        readonly IMoveExecutorSwitcher _moveExecutorSwitcher;
        readonly IHoldActionExecutor _holdActionExecutor;
        readonly IDashExecutor _dashExecutor;
        readonly IPassActionExecutor _passActionExecutor;
        readonly IOnDamageExecutor _onDamageExecutor;


        public Character(
            IMoveExecutorSwitcher moveExecutorSwitcher,
            IHoldActionExecutor holdActionExecutor,
            IDashExecutor dashExecutor,
            IPassActionExecutor passActionExecutor,
            IOnDamageExecutor onDamageExecutor,
            PlayerHoldingObjectContainer holdingObjectContainer
            )
        {
            _moveExecutorSwitcher = moveExecutorSwitcher;
            _holdActionExecutor = holdActionExecutor; 
            _passActionExecutor = passActionExecutor;
            _onDamageExecutor = onDamageExecutor;
            PlayerHoldingObjectContainer = holdingObjectContainer;
            _dashExecutor = dashExecutor;
        }

        public void Reset()
        {
            _holdActionExecutor.Reset();
            _passActionExecutor.Reset();
        }

        public void Setup(PlayerInfo info)
        {
            _moveExecutorSwitcher.Setup(info);
            _holdActionExecutor. Setup(info);
            _passActionExecutor.Setup(info);
            _onDamageExecutor.Setup(info);
            info.PlayerRb.useGravity = true;
        }

        // MoveExecutorContainer
        public void Move(Vector3 direction)
        {
            _moveExecutorSwitcher.Move(direction);
        }
        public void SwitchToBeforeMoveExecutor() => _moveExecutorSwitcher.SwitchToBeforeMoveExecutor();
        public void SwitchToRegularMove() => _moveExecutorSwitcher.SwitchToRegularMove();
        public void SwitchToDashMove() => _moveExecutorSwitcher.SwitchToDashMove();
        public void SwitchToSlowMove() => _moveExecutorSwitcher.SwitchToSlowMove();
        public void SwitchToConfusionMove() => _moveExecutorSwitcher.SwitchToConfusionMove();
        public void SwitchToFaintedMove() => _moveExecutorSwitcher.SwitchToFaintedMove();
        
        public void OnDamage() => _onDamageExecutor.OnDamage();

        // HoldActionExecutor
        public void PutDownBlock() => _holdActionExecutor.PutDownBlock();
        public void SetPlayerBlockPresenter(IPlayerBlockPresenter presenter)
        {
            _holdActionExecutor.SetPlayerBlockPresenter(presenter);
            _passActionExecutor.SetPlayerBlockPresenter(presenter);
        }
        
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _moveExecutorSwitcher.SetPlayerAnimatorPresenter(presenter);
            _holdActionExecutor.SetPlayerAnimatorPresenter(presenter);
            _passActionExecutor.SetPlayerAnimatorPresenter(presenter);
            _onDamageExecutor.SetPlayerAnimatorPresenter(presenter);
        }
        
        public void SetPlayerAidKitPresenter(PlayerAidKitPresenterNet presenter)
        {
            _holdActionExecutor.SetPlayerAidKitPresenter(presenter);
        }

        public void HoldAction()
        {
            _holdActionExecutor.HoldAction();
        }

        // DashExecutor
        public void Dash() => _dashExecutor.Dash();
        
        public void SetDashEffectPresenter(DashEffectPresenter presenter) => _dashExecutor.SetDashEffectPresenter(presenter);

        // PassActionExecutor
        public void PassAction() => _passActionExecutor.PassAction();
        public bool CanReceivePass() => _passActionExecutor.CanReceivePass();
        public void ReceivePass(ICarriableBlock block) => _passActionExecutor.ReceivePass(block);
        
        // IOnDamageExecutor
        public bool IsFainted => _onDamageExecutor.IsFainted;   
        public void OnRevive() => _onDamageExecutor.OnRevive();
        public void SetReviveEffectPresenter(ReviveEffectPresenter presenter) => _onDamageExecutor.SetReviveEffectPresenter(presenter);
    }
}