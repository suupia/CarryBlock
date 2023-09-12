﻿using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class FaintedMoveExecutor : IMoveExecutor
    {
        PlayerInfo _info = null!;
        readonly float _acceleration = 30f;
        readonly float _maxVelocity = 3f; // CorrectlyStopの半分以下
        readonly float _stoppingForce = 5f;

        IPlayerAnimatorPresenter? _playerAnimatorPresenter;

        public void Setup(PlayerInfo info)
        {
            _info = info;
        }

        public void Move(Vector3 input)
        {
            // CannonBallなどからダメージを受けたときの動き
            // 全く動けないか、動けても遅い動き
            if (input != Vector3.zero)
            {
                _playerAnimatorPresenter?.Walk();   
            }
            else
            {
                _playerAnimatorPresenter?.Idle();
            }
        }
        
        // Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _playerAnimatorPresenter = presenter;
        }
    }
}