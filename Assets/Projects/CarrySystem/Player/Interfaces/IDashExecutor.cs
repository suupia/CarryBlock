using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Scripts;
using UnityEngine;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IDashExecutor
    {
        void Setup(PlayerInfo info);
        void Dash();
        public void SetDashEffectPresenter(DashEffectPresenter presenter);

    }
}