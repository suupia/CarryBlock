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
        public PlayerBlockContainer PlayerBlockContainer { get; }
        public PlayerPresenterContainer PresenterContainer { get; }

        readonly IMoveExecutorSwitcher _moveExecutorSwitcher;
        readonly IHoldActionExecutor _holdActionExecutor;
        readonly IDashExecutor _dashExecutor;
        readonly IPassActionExecutor _passActionExecutor;


        public Character(
            IMoveExecutorSwitcher moveExecutorSwitcher,
            IHoldActionExecutor holdActionExecutor,
            IDashExecutor dashExecutor,
            IPassActionExecutor passActionExecutor,
            PlayerBlockContainer blockContainer,
            PlayerPresenterContainer playerPresenterContainer)
        {
            _moveExecutorSwitcher = moveExecutorSwitcher;
            _holdActionExecutor = holdActionExecutor; 
            _passActionExecutor = passActionExecutor;
            PlayerBlockContainer = blockContainer;
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
            info.PlayerRb.useGravity = true;
        }

        // MoveExecutorContainer
        public void Move(Vector3 direction)
        {
            _moveExecutorSwitcher.Move(direction);
        }

        public void SetRegularMoveExecutor() => _moveExecutorSwitcher.SetRegularMoveExecutor();
        public void SetFastMoveExecutor() => _moveExecutorSwitcher.SetFastMoveExecutor();
        public void SetSlowMoveExecutor() => _moveExecutorSwitcher.SetSlowMoveExecutor();

        // HoldActionExecutor
        public void SetHoldPresenter(IPlayerBlockPresenter presenter)
        {
            PresenterContainer.SetHoldPresenter(presenter);
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
        public void ReceivePass(IBlock block) => _passActionExecutor.ReceivePass(block);
    }
}