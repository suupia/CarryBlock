using UnityEngine;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Scripts;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IHoldActionExecutor
    {
        void Setup(PlayerInfo info);
        void Reset();
        void HoldAction();

        public void SetPlayerBlockPresenter(IPlayerBlockPresenter presenter);
        public void SetPlayerAidKitPresenter(PlayerAidKitPresenterNet presenter);
    }
}