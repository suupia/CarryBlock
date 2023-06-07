using Fusion;
using Main;
using Nuts.BattleSystem.Move.Scripts;
using UnityEngine;

namespace Nuts.BattleSystem.Player.Scripts
{
    public class Tank : IUnit
    {
        readonly IUnitAction _action;
        readonly PlayerInfo _info;
        readonly IMove _move;
        readonly NetworkRunner _runner;

        public Tank(PlayerInfo info)
        {
            _info = info;
            _runner = info._runner;
            _move = new RegularMove
            {
                transform = _info.playerObj.transform,
                rd = _info.playerRd,
                acceleration = _info.acceleration,
                maxVelocity = _info.maxVelocity,
                targetRotationTime = _info.targetRotationTime,
                maxAngularVelocity = _info.maxAngularVelocity
            };
            _action = new EjectPicker(_runner, _info.playerObj, _info.playerInfoForPicker);
            _info.playerRd.useGravity = true;
        }

        public void Move(Vector3 direction)
        {
            _move.Move(direction);
        }

        public void Action()
        {
            _action.Action();
        }

        public bool InAction()
        {
            return _action.InAction();
        }

        public float ActionCooldown()
        {
            return _action.ActionCooldown();
        }
    }
}