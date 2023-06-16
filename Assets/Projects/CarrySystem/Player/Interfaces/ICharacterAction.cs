using UnityEngine;
using Carry.CarrySystem.Player.Info;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface ICharacterAction
    {
        void Setup();
        void Action(PlayerInfo info);
    }
}