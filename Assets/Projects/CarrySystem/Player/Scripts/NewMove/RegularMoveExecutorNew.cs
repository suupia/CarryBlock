﻿#nullable enable
using System;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;


namespace Carry.CarrySystem.Player.Scripts
{
    public class RegularMoveExecutorNew : IMoveExecutorNew
    {
        PlayerInfo? _info;
        IPlayerAnimatorPresenter? _playerAnimatorPresenter;
        readonly IMoveParameter _moveParameter;

        public RegularMoveExecutorNew(float acceleration, float maxVelocity, float stoppingForce)
        {
            _moveParameter = new MoveParameter(acceleration, maxVelocity, stoppingForce);
        }

        public void SetUp(PlayerInfo info)
        {
            _info = info;
        }
        
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _playerAnimatorPresenter = presenter;
        }

        public IMoveParameter Chain(IMoveParameter _)
        {
            return _moveParameter;
        }

        class MoveParameter : IMoveParameter
        {
            public float Acceleration { get; set; }
            public float MaxVelocity { get; set; }
            public float StoppingForce { get; set; }

            public MoveParameter(float acceleration, float maxVelocity, float stoppingForce)
            {
                Acceleration = acceleration;
                MaxVelocity = maxVelocity;
                StoppingForce = stoppingForce;
            }
        }

        public IMoveFunction Chain(IMoveFunction _)
        {
            if (_info == null) throw new NullReferenceException($"PlayerInfo is null.");
            if (_playerAnimatorPresenter == null) throw new NullReferenceException($"PlayerAnimatorPresenter is null.");
            return new MoveFunction(_playerAnimatorPresenter, _info,_moveParameter );
        }

        class MoveFunction : IMoveFunction
        {
            readonly IPlayerAnimatorPresenter _presenter;
            readonly PlayerInfo _info;
            readonly IMoveParameter _parameter;

            public MoveFunction(IPlayerAnimatorPresenter presenter,PlayerInfo info, IMoveParameter parameter)
            {
                _presenter = presenter;
                _info = info;
                _parameter = parameter;
            }

            public void Move(Vector3 input)
            {
                var transform = _info.PlayerObj.transform;
                var rb = _info.PlayerRb;

                var deltaAngle = Vector3.SignedAngle(transform.forward, input, Vector3.up);
                // Debug.Log($"deltaAngle = {deltaAngle}");

                if (input != Vector3.zero)
                {
                    // Rotate if there is a difference of more than Epsilon degrees
                    if (Mathf.Abs(deltaAngle) >= float.Epsilon)
                    {
                        var rotateQuaternion = Quaternion.Euler(0, deltaAngle, 0);
                        rb.MoveRotation(rb.rotation * rotateQuaternion);
                    }

                    rb.AddForce(_parameter.Acceleration * input, ForceMode.Acceleration);

                    if (rb.velocity.magnitude >= _parameter.MaxVelocity)
                        rb.velocity = _parameter.MaxVelocity * rb.velocity.normalized;
                }
                else
                {
                    // Stop if there is no key input
                    // Define 0 < _stoppingForce < 1
                    float reductionFactor = Mathf.Max(0f, 1f - _parameter.StoppingForce * Time.deltaTime);
                    float stoppingSpeed = 1.5f;

                    rb.velocity *= Mathf.Pow(reductionFactor, rb.velocity.magnitude);

                    if (rb.velocity.magnitude <= stoppingSpeed)
                    {
                        rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
                        rb.angularVelocity = new Vector3(0f, rb.angularVelocity.y, 0f);
                    }
                }

                if (input != Vector3.zero)
                {
                    _presenter.Walk();
                }
                else
                {
                    _presenter.Idle();
                }
            }
        }



    }
}