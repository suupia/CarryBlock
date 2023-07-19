using Fusion;
using Projects.Utility.Scripts;
using Projects.BattleSystem.Move.Scripts;
using UnityEngine;

namespace Projects.BattleSystem.Player.Scripts
{
    public class CollectResourcePlane : IUnit
    {
        readonly IUnitAction _action;
        readonly PlayerInfo _info;
        readonly IMove _move;
        readonly NetworkRunner _runner;

        public CollectResourcePlane(PlayerInfo info)
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
            _action = new CollectResource(_runner, _info.playerObj);
            _info.playerRd.useGravity = false;
        }

        public void Move(Vector3 direction)
        {
            if (InAction()) return;

            _move.Move(direction);
        }

        public bool InAction()
        {
            return _action.InAction();
        }

        public float ActionCooldown()
        {
            return _action.ActionCooldown();
        }

        public void Action()
        {
            _action.Action();
        }
    }

    public class EstablishSubBasePlane : IUnit
    {
        readonly IUnitAction _action;
        readonly PlayerInfo _info;
        readonly IMove _move;
        readonly NetworkRunner _runner;

        public EstablishSubBasePlane(PlayerInfo info)
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
            _action = new EstablishSubBase(_runner, _info.playerObj);
            _info.playerRd.useGravity = false;
        }

        public void Move(Vector3 direction)
        {
            if (InAction()) return;

            _move.Move(direction);
        }

        public bool InAction()
        {
            return _action.InAction();
        }

        public float ActionCooldown()
        {
            return _action.ActionCooldown();
        }

        public void Action()
        {
            _action.Action();
        }
    }
}