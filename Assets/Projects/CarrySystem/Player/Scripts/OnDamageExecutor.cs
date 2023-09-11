using System;
using Carry.CarrySystem.CG.Tsukinowa;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    using System.Threading;
    using Cysharp.Threading.Tasks;

    public class OnDamageExecutor : IOnDamageExecutor
    {
        public bool IsFainted { get; private set; }
        PlayerInfo _info = null!;
        readonly float _faintDuration = 10.0f;
        readonly IMoveExecutorSwitcher _moveExecutorSwitcher;
        CancellationTokenSource? _cancellationTokenSource;

        public OnDamageExecutor(IMoveExecutorSwitcher moveExecutorSwitcher)
        {
            _moveExecutorSwitcher = moveExecutorSwitcher;
        }

        public void Setup(PlayerInfo info)
        {
            _info = info;
        }

        public void OnDamage()
        {
            Debug.Log($"ダメージを受けた！");
            _cancellationTokenSource = new CancellationTokenSource();
            var _ = Faint(_cancellationTokenSource.Token);
        }

        public void OnRevive()
        {
            if (IsFainted)
            {
                _cancellationTokenSource?.Cancel();
                IsFainted = false;
                // 他の復帰処理もここで行うことができます。
            }
        }

        async UniTaskVoid Faint(CancellationToken cancellationToken)
        {
            try
            {
                Debug.Log($"気絶する");
                IsFainted = true;
                _moveExecutorSwitcher.SwitchToFaintedMove();
                Debug.Log($"_info.PlayerObj.GetComponentInChildren<TsukinowaMaterialSetter>()) : {_info.PlayerObj.GetComponentInChildren<TsukinowaMaterialSetter>()}");
                _info.PlayerObj.GetComponentInChildren<TsukinowaMaterialSetter>().Blinking();
                await UniTask.Delay((int)(_faintDuration * 1000), cancellationToken: cancellationToken);
                Debug.Log($"気絶から復帰する");
                IsFainted = false;
                _moveExecutorSwitcher.SwitchToRegularMove();
            }
            catch (OperationCanceledException)
            {
                // キャンセル処理
                Debug.Log($"気絶がキャンセルされました");
                IsFainted = false; // 一応
            }
        }
    }

}