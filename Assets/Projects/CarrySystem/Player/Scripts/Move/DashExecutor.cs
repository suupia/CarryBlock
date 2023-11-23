using System;
using System.Threading;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.VFX.Interfaces;
using Carry.CarrySystem.VFX.Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class DashExecutor : IDashExecutor
    {
        PlayerInfo _info = null!;
        readonly IMoveExecutorSwitcherNew _moveExecutorSwitcher;
        readonly IOnDamageExecutor _onDamageExecutor;
        IDashEffectPresenter? _dashEffectPresenter;
        readonly float _dashTime = 0.6f;
        readonly float _dashCoolTime = 1f;
        bool _isDashing;
        
        CancellationTokenSource? _cancellationTokenSource;
        
        public DashExecutor(
            IMoveExecutorSwitcherNew moveExecutorSwitcher,
            IOnDamageExecutor onDamageExecutor
            )
        {
            _moveExecutorSwitcher = moveExecutorSwitcher;
            _onDamageExecutor = onDamageExecutor;
        }
        
        public void Setup(PlayerInfo info)
        {
            _info = info;
        }

        public void Dash()
        {
            if (_isDashing) return;
            if(_onDamageExecutor.IsFainted) return;
            
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var _ = AsyncDash(_cancellationTokenSource.Token);
        }
        
        async UniTaskVoid AsyncDash(CancellationToken cancellationToken)
        {
            try
            {
                ChangeDashMove();
                await UniTask.Delay((int)(_dashTime * 1000), cancellationToken: cancellationToken);
                ChangeBeforeMove();
                await UniTask.Delay((int)(_dashCoolTime * 1000), cancellationToken: cancellationToken);
                _isDashing = false;
            }
            catch (OperationCanceledException)
            {
                _isDashing = false;
            }
        }

        void ChangeDashMove()
        {
            Debug.Log($"Executing Dash");
            _isDashing = true;
            _moveExecutorSwitcher.SwitchToDashMove();
            if(_dashEffectPresenter != null) _dashEffectPresenter.StartDash();
        }

        void ChangeBeforeMove()
        {
            Debug.Log($"Finish dashing");
            if(_dashEffectPresenter != null) _dashEffectPresenter.StopDash();
            if(_onDamageExecutor.IsFainted) return;
            _moveExecutorSwitcher.SwitchOffDashMove();
        }
        
        public void SetDashEffectPresenter(IDashEffectPresenter presenter)
        {
            _dashEffectPresenter = presenter;
        }
    }
} 