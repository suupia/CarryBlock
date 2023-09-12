using Carry.CarrySystem.Player.Info;
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
        readonly float _cannonStoppingForce = 3f;

        IPlayerAnimatorPresenter? _playerAnimatorPresenter;

        public void Setup(PlayerInfo info)
        {
            _info = info;
        }

        public void Move(Vector3 input)
        {
            // CannonBallなどからダメージを受けたときの動き
            // 全く動けないか、動けても遅い動き
            
            var rb = _info.PlayerRb;
            float reductionFactor = Mathf.Max(0f, 1f - _cannonStoppingForce * Time.deltaTime);
            float stoppingSpeed = 1.5f;

            rb.velocity *= Mathf.Pow(reductionFactor, rb.velocity.magnitude);
                
            if (rb.velocity.magnitude <= stoppingSpeed )
            {
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
                rb.angularVelocity = new Vector3(0f, rb.angularVelocity.y, 0f);
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