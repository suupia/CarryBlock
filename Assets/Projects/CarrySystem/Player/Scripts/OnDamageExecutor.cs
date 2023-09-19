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
        public float FaintedSeconds => CalcFaintedTime();
        PlayerInfo _info = null!;
        readonly IMoveExecutorSwitcher _moveExecutorSwitcher;
        readonly PlayerCharacterTransporter _playerCharacterTransporter;
        
        IPlayerAnimatorPresenter? _playerAnimatorPresenter;
        ReviveEffectPresenter? _reviveEffectPresenter;
        
        CancellationTokenSource? _cancellationTokenSource;

        public OnDamageExecutor(
            IMoveExecutorSwitcher moveExecutorSwitcher,
            PlayerCharacterTransporter playerCharacterTransporter
            )
        {
            _moveExecutorSwitcher = moveExecutorSwitcher;
            _playerCharacterTransporter = playerCharacterTransporter;
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
                await UniTask.Delay((int)(FaintedSeconds * 1000), cancellationToken: cancellationToken);
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
            _moveExecutorSwitcher.SwitchToBeforeMoveExecutor();
            _playerAnimatorPresenter?.Revive();
            if (_reviveEffectPresenter != null) _reviveEffectPresenter.StartRevive();
        }
        
        float CalcFaintedTime()
        {
            float faintedTime = _playerCharacterTransporter.PlayerCount switch
            {
                1 => 2,
                2 => 3,
                3 => 3.5f,
                4 => 4f,
                _ =>  InvalidPlayerCount(),
            };
            
            return faintedTime;
            
            float InvalidPlayerCount()
            {
                Debug.LogError($"PlayerCount : {_playerCharacterTransporter.PlayerCount} is invalid.");
                return 5f;
            }
        }
        
        //Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _playerAnimatorPresenter = presenter;
        }
        
        public void SetReviveEffectPresenter(ReviveEffectPresenter presenter)
        {
            _reviveEffectPresenter = presenter;
        }
    }
}