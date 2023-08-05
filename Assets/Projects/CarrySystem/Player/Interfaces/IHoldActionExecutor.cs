using UnityEngine;
using Carry.CarrySystem.Player.Info;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IHoldActionExecutor
    {
        void SetHoldPresenter(IPlayerBlockPresenter presenter);
        void Setup(PlayerInfo info);
        void Reset();
        void HoldAction();
    }
}