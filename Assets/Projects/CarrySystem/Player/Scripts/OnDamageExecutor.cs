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
        readonly float _faintSeconds = 60.0f;  // ほぼ無限の気絶時間という感じ
        readonly IMoveExecutorSwitcher _moveExecutorSwitcher;
        CancellationTokenSource? _cancellationTokenSource;
        
        IPlayerAnimatorPresenter? _playerAnimatorPresenter;

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
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var _ = AsyncOnDamaged(_cancellationTokenSource.Token);
        }

        public void OnRevive()
        {
            Debug.Log($"復活する");
            if (IsFainted)
            {
                _cancellationTokenSource?.Cancel();
                Revive();
            }
        }

        async UniTaskVoid AsyncOnDamaged(CancellationToken cancellationToken)
        {
            try
            {
                Faint();
                await UniTask.Delay((int)(_faintSeconds * 1000), cancellationToken: cancellationToken);
                Revive();
            }
            catch (OperationCanceledException)
            {
                // Do Nothing
            }
        }

        void Faint()
        {
            Debug.Log($"気絶する");
            IsFainted = true;
            _moveExecutorSwitcher.SwitchToFaintedMove();
            Debug.Log(
                $"_info.PlayerObj.GetComponentInChildren<TsukinowaMaterialSetter>()) : {_info.PlayerObj.GetComponentInChildren<TsukinowaMaterialSetter>()}");
            _info.PlayerObj.GetComponentInChildren<TsukinowaMaterialSetter>().Blinking();
            _playerAnimatorPresenter?.Faint();
        }

        void Revive()
        {
            Debug.Log($"気絶から復帰する");
            IsFainted = false;
            _moveExecutorSwitcher.SwitchToRegularMove();
            _playerAnimatorPresenter?.Revive();
        }
        
        //Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _playerAnimatorPresenter = presenter;
        }
    }
}