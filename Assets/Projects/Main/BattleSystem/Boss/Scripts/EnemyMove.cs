using System.Collections;
using System.Collections.Generic;
using Fusion;
using Main;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

namespace Boss
{
    /// <summary>
    /// IInputMoveExecutorにランダムな入力をinputとして与える
    /// </summary>
    public class RandomMove : IEnemyMoveExecutor
    {
        readonly float _simulationInterval = 2f;
        readonly IInputMoveExecutor _move;
        Vector3 _simulatedInput;
        bool _isSimulating;
        readonly CancellationTokenSource _cancellationTokenSource;

        public RandomMove(float simulationInterval, IInputMoveExecutor move)
        {
            _simulationInterval = simulationInterval;
            _move = move;
            _cancellationTokenSource = new CancellationTokenSource();
        }
        
        public void Move()
        {
            InputSimulation();
            _move.Move(_simulatedInput);
        }
        
        async UniTaskVoid InputSimulation()
        {
            if(_isSimulating)return;
            _isSimulating = true;
            _simulatedInput = MoveUtility.SimulateRandomInput();
            await UniTask.Delay(TimeSpan.FromSeconds(_simulationInterval),
                cancellationToken: _cancellationTokenSource.Token);
            _isSimulating = false;
        }
    }

    /// <summary>
    /// IInputMoveExecutorに_transform.forwardをinputとして与える
    /// </summary>
    public class ForwardMove : IEnemyMoveExecutor
    {
        readonly Transform _transform;
        readonly IInputMoveExecutor _move;
        public ForwardMove(Transform transform, IInputMoveExecutor move)
        {
            _transform = transform;
            _move = move;
        }
        public void Move()
        {
            var input = _transform.forward;
            _move.Move(input);
        }
    }

    /// <summary>
    /// IInputMoveExecutorにtargetの方向をinputとして与える
    /// </summary>
    public class TargetMove : IEnemyMoveExecutor
    {
        readonly Transform _transform;
        readonly Transform _target;
        readonly IInputMoveExecutor _move;
        public TargetMove(Transform transform, Transform target, IInputMoveExecutor move)
        {
            _transform = transform;
            _target = target;
            _move = move;
        }
        
        public void Move()
        {
            var input = (_target.position - _transform.position).normalized;
            _move.Move(input);
        }
    }

    public interface IInputMoveExecutor
    {
        void Move(Vector3 input); // 正規化して与える必要はない
    }

    /// <summary>
    /// 何もしないIInputMoveExecutorの実装クラス
    /// </summary>
    public class DoNothingMove : IInputMoveExecutor
    {
        public DoNothingMove()
        {
        }
        public void Move(Vector3 input)
        {
        }
    }

    /// <summary>
    /// inputの方を向く
    /// </summary>
    public class LookAtInputMoveDecorator : IInputMoveExecutor
    {
        readonly Transform _transform;
        readonly Transform _target;
        readonly IInputMoveExecutor _move;
        public LookAtInputMoveDecorator(Transform transform, Transform target, IInputMoveExecutor move)
        {
            _transform = transform;
            _target = target;
            _move = move;
        }
        public void Move(Vector3 input)
        {
            var nextWorldPos = input + _transform.position;
            _transform.LookAt(nextWorldPos);
            _move.Move(input);
        }
    }
    
    /// <summary>
    /// targetの方を向く
    /// </summary>
    public class LookAtTargetMoveDecorator : IInputMoveExecutor
    {
        readonly Transform _transform;
        readonly Transform _target;
        readonly IInputMoveExecutor _move;
        public LookAtTargetMoveDecorator(Transform transform, Transform target, IInputMoveExecutor move)
        {
            _transform = transform;
            _target = target;
            _move = move;
        }
        public void Move(Vector3 input)
        {
            _transform.LookAt(_target);
            _move.Move(input);
        }
    }
    
    /// <summary>
    /// RigidBodyで動く
    /// velocityの制限はx,zのみにかける
    /// </summary>
    public class AddForceMove : IInputMoveExecutor  // ToDo: AddTorqueMoveのようなクラスの必要性について考える
    {
        Rigidbody _rb;
        public float acceleration { get; set; } = 30f;
        public float maxVelocity { get; set; } = 8f;

        public AddForceMove( Rigidbody rb)
        {
            _rb = rb;
        }

        public void Move(Vector3 input)
        {
            var unitInput = input.normalized;
            _rb.AddForce(input * acceleration, ForceMode.Acceleration);

            var velocity = Utility.SetYToZero(_rb.velocity);
            if (velocity.magnitude >= maxVelocity)
            {
                velocity = maxVelocity * velocity.normalized;
                velocity.y = _rb.velocity.y;
                _rb.velocity = velocity;
            }
        }
    }
}