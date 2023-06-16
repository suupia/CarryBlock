using Carry.CarrySystem.Player.Info;
using UnityEngine;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface ICharacterMove
    {
        void Setup(PlayerInfo info);
        void Move(Vector3 input);
    }
}