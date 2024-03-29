using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.CarriableBlock.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using UnityEngine;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.VFX.Interfaces;
using Carry.CarrySystem.VFX.Scripts;

#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class Character :      
        IMoveExecutorSwitcher, 
        IHoldActionExecutor, 
        IOnDamageExecutor,
        IDashExecutor,
        IPassActionExecutor
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
            if(info.PlayerRb == null ) Debug.LogError("info.PlayerRb == null");
            info.PlayerRb.useGravity = true;
        }

        // MoveExecutorContainer
        public void Move(Vector3 direction)
        {
            _moveExecutorSwitcher.Move(direction);
        }
        public void AddMoveRecord<T>() where T : IMoveChainable => _moveExecutorSwitcher.AddMoveRecord<T>();

        public void RemoveRecord<T>() where T : IMoveChainable => _moveExecutorSwitcher.RemoveRecord<T>();
        
        public void OnDamage() => _onDamageExecutor.OnDamage();

        // HoldActionExecutor
        public void ResetHoldingBlock() => _holdActionExecutor.ResetHoldingBlock();
        public void SetPlayerBlockPresenter(IPlayerHoldablePresenter presenter)
        {
            _holdActionExecutor.SetPlayerBlockPresenter(presenter);
            _passActionExecutor.SetPlayerBlockPresenter(presenter);
        }
        
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _moveExecutorSwitcher.SetPlayerAnimatorPresenter(presenter);
            _holdActionExecutor.SetPlayerAnimatorPresenter(presenter);
            _onDamageExecutor.SetPlayerAnimatorPresenter(presenter);
            _passActionExecutor.SetPlayerAnimatorPresenter(presenter);
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
        
        public void SetDashEffectPresenter(IDashEffectPresenter presenter) => _dashExecutor.SetDashEffectPresenter(presenter);

        // PassActionExecutor
        public void PassAction() => _passActionExecutor.PassAction();
        public bool CanReceivePass() => _passActionExecutor.CanReceivePass();
        public void ReceivePass(ICarriableBlock block) => _passActionExecutor.ReceivePass(block);
        public void SetPassBlockMoveExecutor(PassBlockMoveExecutorNet presenter) => _passActionExecutor.SetPassBlockMoveExecutor(presenter);
        
        // IOnDamageExecutor
        public bool IsFainted => _onDamageExecutor.IsFainted;   
        public void OnRevive() => _onDamageExecutor.OnRevive();
        public void SetReviveEffectPresenter(ReviveEffectPresenter presenter) => _onDamageExecutor.SetReviveEffectPresenter(presenter);
    }
}