using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using UnityEngine;

namespace Carry.CarrySystem.Player.Scripts
{
    public class Character : ICharacter
    {
        readonly ICharacterAction _action;
        readonly ICharacterMove _move;
        readonly NetworkRunner _runner;

        public Character(PlayerInfo info)
        {
            _runner = info.runner;
            _move = new QuickTurnMove(info.playerObj.transform, info.playerRb)
            {
                acceleration = info.acceleration,
                maxVelocity = info.maxVelocity,
                rotateTime = info.targetRotationTime,
            };
            _action = new CharacterAction(info.playerObj.transform);
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