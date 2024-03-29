﻿#nullable enable

using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Scripts;
using Carry.CarrySystem.VFX.Scripts;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IOnDamageExecutor
    {
        public bool IsFainted { get; }
        public void Setup(PlayerInfo info);
        public void OnDamage();

        public void OnRevive();
        
        // Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter);
        
        public void SetReviveEffectPresenter(ReviveEffectPresenter presenter);

    }
}