using UnityEngine;
using Carry.CarrySystem.Player.Info;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface ICharacterHoldAction
    {
        void SetHoldPresenter(IHoldActionPresenter presenter);
        void Setup(PlayerInfo info);
        void Reset();
        void Action();
    }
}