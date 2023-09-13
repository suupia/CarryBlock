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
        readonly IMoveExecutorSwitcher _moveExecutorSwitcher;
        float _dashTime;
        bool _isDashing;
        PlayerInfo _info = null!;
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
                ChangeMoveFast();
                await UniTask.Delay((int)(_dashTime * 1000), cancellationToken: cancellationToken);
                RechangeMove();
            }
            catch (OperationCanceledException)
            {
                // Do Nothing
            }
        }

        void ChangeMoveFast()
        {
            Debug.Log($"Executing Dash");
            _isDashing = true;
            _moveExecutorSwitcher.SwitchToFastMove();
        }

        void RechangeMove();
        {
            Debug.Log($"Finish dashing");
            _isDashing = false;
            _moveExecutorSwitcher.SwitchToRegularMove();
        }
    }
} 