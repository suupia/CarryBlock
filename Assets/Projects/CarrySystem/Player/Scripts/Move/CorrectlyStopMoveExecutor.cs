﻿using System.Security.Cryptography.X509Certificates;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class CorrectlyStopMoveExecutor : IMoveExecutor
    {
        PlayerInfo _info = null!;
        readonly float _acceleration = 40f;
        readonly float _maxVelocity = 5f;
        readonly float _stoppingForce = 5f;

        IPlayerAnimatorPresenter? _playerAnimatorPresenter;

        public void Setup(PlayerInfo info)
        {
            _info = info;
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

                rb.AddForce(_acceleration * input, ForceMode.Acceleration);

                if (rb.velocity.magnitude >= _maxVelocity)
                    rb.velocity = _maxVelocity * rb.velocity.normalized;
            }
            else
            {
                // Stop if there is no key input
                // Define 0 < _stoppingForce < 1
                float reductionFactor = Mathf.Max(0f, 1f - _stoppingForce * Time.deltaTime);
                float stoppingSpeed = 1.5f;

                rb.velocity *= Mathf.Pow(reductionFactor, rb.velocity.magnitude);
                
                if (rb.velocity.magnitude <= stoppingSpeed )
                {
                    rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
                    rb.angularVelocity = new Vector3(0f, rb.angularVelocity.y, 0f);
                }
            }

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