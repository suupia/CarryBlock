using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Main
{
    public class CollectResourcePlane : IUnit
    {
        readonly NetworkRunner _runner;
        PlayerInfo _info;
        IMove _move;
        IUnitAction _action;

        public CollectResourcePlane(PlayerInfo info)
        {
            _info = info;
            _runner = info._runner;
            _move = new RegularMove()
            {
                transform = _info.playerObj.transform,
                rd = _info.playerRd,
                acceleration = _info.acceleration,
                maxVelocity = _info.maxVelocity,
                targetRotationTime = _info.targetRotationTime,
                maxAngularVelocity = _info.maxAngularVelocity,
            };
            _action = new CollectResource(_runner, _info.playerObj);
            _info.playerRd.useGravity = false;
        }

        public void Move(Vector3 direction)
        {
            if (InAction()) return;

            _move.Move(direction);
        }

        public bool InAction() => _action.InAction();

        public float ActionCooldown() => _action.ActionCooldown();

        public void Action() => _action.Action();
    }
    
    public class EstablishSubBasePlane : IUnit
    {
        readonly NetworkRunner _runner;
        PlayerInfo _info;
        IMove _move;
        IUnitAction _action;

        public EstablishSubBasePlane(PlayerInfo info)
        {
            _info = info;
            _runner = info._runner;
            _move = new RegularMove()
            {
                transform = _info.playerObj.transform,
                rd = _info.playerRd,
                acceleration = _info.acceleration,
                maxVelocity = _info.maxVelocity,
                targetRotationTime = _info.targetRotationTime,
                maxAngularVelocity = _info.maxAngularVelocity,
            };
            _action = new EstablishSubBase(_runner, _info.playerObj);
            _info.playerRd.useGravity = false;
        }

        public void Move(Vector3 direction)
        {
            if (InAction()) return;

            _move.Move(direction);
        }

        public bool InAction() => _action.InAction();

        public float ActionCooldown() => _action.ActionCooldown();

        public void Action() => _action.Action();
    }
}