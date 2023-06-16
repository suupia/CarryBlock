using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using UnityEngine;
using Carry.CarrySystem.Player.Info;

#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class Character : ICharacter
    {
        readonly ICharacterAction _action;
        readonly ICharacterMove _move;
        
        public Character(ICharacterMove move, ICharacterAction action)
        {
            _move = move;
            _action = action;
        }

        public void Setup(PlayerInfo info)
        {
            _move.Setup(info);
            _action.Setup(info);
            info.playerRb.useGravity = true;
        }

        public void Move(Vector3 direction)
        {
            _move.Move(direction);
        }

        public void Action()
        {
            _action.Action();
        }
    }
}