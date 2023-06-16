using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using UnityEngine;
using Carry.CarrySystem.Player.Info;
using VContainer.Unity;
using VContainer;

#nullable enable

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
            // _move = new QuickTurnMove()
            // {
            //     acceleration = info.acceleration,
            //     maxVelocity = info.maxVelocity,
            //     rotateTime = info.targetRotationTime,
            // };
            // _action = new CharacterAction();
            var resolver = Object.FindObjectOfType<LifetimeScope>().Container;
            _move = resolver.Resolve<QuickTurnMove>();
            _action = resolver.Resolve<CharacterAction>();
            info.playerRb.useGravity = true;
        }
        
        public void Setup(PlayerInfo info)
        {
            _move.Setup(info);
            _action.Setup( info);
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