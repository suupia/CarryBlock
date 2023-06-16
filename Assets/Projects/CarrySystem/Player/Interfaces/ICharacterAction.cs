using UnityEngine;
using Carry.CarrySystem.Player.Info;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface ICharacterAction
    {
        void Setup(PlayerInfo info);
        void Action();
    }
}