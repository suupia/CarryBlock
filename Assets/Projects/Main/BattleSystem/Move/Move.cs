using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main
{
    public interface IMove
    {
        void Move(Vector3 input = default);
    }

    public class StableMove : IMove
    {
        public void Move(Vector3 input = default)
        {
            //Ignore
        }
    }

    /// <summary>
    /// ふらふら歩く動き
    /// 引数を指定しない想定
    /// 引数を指定すると、moveの動きになるが、あまり使用する意味はない
    /// ただし、実装の関係上、Vector.zeroに対応できない
    /// 動きたくない場合は、StableMoveを使用する
    /// </summary>
    public class WanderingMove : IMove, IDisposable
    {
            
        public struct Context
        {
            public float InputSimulationFrequency;
        }
        
        private IMove _move;
        private Context _context;
        private Vector3 _simulatedInput;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public WanderingMove(IMove move) :
            this(move, new Context()
            {
                InputSimulationFrequency = 2f
            })
        {
            
        }

        public WanderingMove(IMove move, Context context)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _move = move;
            _context = context;

            StartSimulation(_cancellationTokenSource.Token).Forget();
        }

        public void Move(Vector3 input = default)
        {
            _move.Move(input == default ? _simulatedInput : input);
        }
        
        private async UniTaskVoid StartSimulation(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_context.InputSimulationFrequency), cancellationToken: cancellationToken);
                _simulatedInput = SimulateRandomInput();
                Debug.Log(_simulatedInput);
            }
        }

        private Vector3 SimulateRandomInput() => new Vector3(
            Random.Range(-1, 2),
            0,
            Random.Range(-1, 2)
        ).normalized;

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
        }
    }

    /// <summary>
    /// Moveの引数に行きたい場所をVector3で渡すとそちらに向かう動き
    /// 動き方はmoveで指定可
    /// </summary>
    public class ToTargetMove : IMove
    {
        private Transform _from;
        private IMove _move;

        public ToTargetMove(Transform from, IMove move)
        {
            _from = from;
            _move = move;
        }

        public void Move(Vector3 target)
        {
            var direction = Utility.SetYToZero(target - _from.position).normalized;
            _move.Move(direction);
        }
    }

    public class RegularMove : IMove
    {
        public Transform transform { get; set; }
        public Rigidbody rd { get; set; }
        public float acceleration { get; set; } = 30f;
        public float maxVelocity { get; set; } = 8f;
        public float targetRotationTime { get; set; } = 0.2f;
        public float maxAngularVelocity { get; set; } = 100f;


        public void Move(Vector3 input)
        {
            var deltaAngle = Vector3.SignedAngle(transform.forward, input, Vector3.up);
            // Debug.Log($"deltaAngle = {deltaAngle}");

            if (input != Vector3.zero)
            {
                // Rotate if there is a difference of more than Epsilon degrees
                if (Mathf.Abs(deltaAngle) >= float.Epsilon)
                {
                    var torque = (2 * deltaAngle) / Mathf.Sqrt(targetRotationTime);
                    rd.AddTorque(torque * Vector3.up, ForceMode.Acceleration);
                }

                if (rd.angularVelocity.magnitude >= rd.maxAngularVelocity)
                    rd.angularVelocity = maxAngularVelocity * rd.angularVelocity.normalized;

                rd.AddForce(acceleration * input, ForceMode.Acceleration);

                if (rd.velocity.magnitude >= maxVelocity)
                    rd.velocity = maxVelocity * rd.velocity.normalized;
            }
        }
    }
}