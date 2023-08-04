using UnityEngine;
using Carry.CarrySystem.Player.Info;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IHoldActionExecutor
    {
        void SetHoldPresenter(IHoldActionPresenter presenter);
        void Setup(PlayerInfo info);
        void Reset();
        void HoldAction();
    }
}