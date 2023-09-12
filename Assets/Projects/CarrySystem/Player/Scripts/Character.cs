using Carry.CarrySystem.Block.Interfaces;
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
        public PlayerPresenterContainer PresenterContainer { get; }

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
            PlayerHoldingObjectContainer holdingObjectContainer,
            PlayerPresenterContainer playerPresenterContainer)
        {
            _moveExecutorSwitcher = moveExecutorSwitcher;
            _holdActionExecutor = holdActionExecutor; 
            _passActionExecutor = passActionExecutor;
            _onDamageExecutor = onDamageExecutor;
            PlayerHoldingObjectContainer = holdingObjectContainer;
            PresenterContainer = playerPresenterContainer;
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

        public void SwitchToRegularMove() => _moveExecutorSwitcher.SwitchToRegularMove();
        public void SwitchToFastMove() => _moveExecutorSwitcher.SwitchToFastMove();
        public void SwitchToSlowMove() => _moveExecutorSwitcher.SwitchToSlowMove();
        public void SwitchToFaintedMove() => _moveExecutorSwitcher.SwitchToFaintedMove();
        
        public void OnDamage() => _onDamageExecutor.OnDamage();

        // HoldActionExecutor
        public void SetHoldPresenter(IPlayerBlockPresenter presenter)
        {
            PresenterContainer.SetHoldPresenter(presenter);
        }
        
        public void SetAidKitPresenter(PlayerAidKitPresenterNet presenter)
        {
            _holdActionExecutor.SetAidKitPresenter(presenter);
        }

        public void HoldAction()
        {
            _holdActionExecutor.HoldAction();
        }

        // DashExecutor
        public void Dash() => _dashExecutor.Dash();

        // PassActionExecutor
        public void PassAction() => _passActionExecutor.PassAction();
        public bool CanReceivePass() => _passActionExecutor.CanReceivePass();
        public void ReceivePass(ICarriableBlock block) => _passActionExecutor.ReceivePass(block);
        
        // IOnDamageExecutor
        public bool IsFainted => _onDamageExecutor.IsFainted;   
        public void OnRevive() => _onDamageExecutor.OnRevive();
    }
}