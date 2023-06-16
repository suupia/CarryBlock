using Carry.CarrySystem.Player.Info;
using UnityEngine;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface ICharacterMove
    {
        void Move(PlayerInfo info, Vector3 input);
    }
}