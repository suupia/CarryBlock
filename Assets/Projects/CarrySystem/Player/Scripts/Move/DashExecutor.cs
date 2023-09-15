using System;
using System.Threading;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class DashExecutor : IDashExecutor
    {
        PlayerInfo _info = null!;
        readonly IMoveExecutorSwitcher _moveExecutorSwitcher;
        DashEffectPresenter? _dashEffectPresenter;
        readonly float _dashTime = 0.25f;
        readonly float _dashCoolTime = 3f;
        bool _isDashing;
        
        CancellationTokenSource? _cancellationTokenSource;
        
        public DashExecutor(IMoveExecutorSwitcher moveExecutorSwitcher)
        {
            _moveExecutorSwitcher = moveExecutorSwitcher;
        }
        
        public void Setup(PlayerInfo info)
        {
            _info = info;
        }

        public void Dash()
        {
            if (_isDashing) return;
            
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
                ChangeRegularMove();
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
            _dashEffectPresenter.StartDash();
        }

        void ChangeRegularMove()
        {
            Debug.Log($"Finish dashing");
            _moveExecutorSwitcher.SwitchToRegularMove();
            _dashEffectPresenter.StopDash();
        }
        
        public void SetDashEffectPresenter(DashEffectPresenter presenter)
        {
            _dashEffectPresenter = presenter;
        }
    }
} 