using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main
{
    public interface IMove
    {
        void Move(Vector3 input = default);
    }

    public interface ITargetMove : IMove
    {
        Transform Target { get; set; }
    }

    /// <summary>
    /// いくつかのIMove実装クラスの基底クラス
    /// _moveプロパティを持ち、
    /// ToStringをオーバーライドしていて、ネスト関係が見やすいようになっている
    /// </summary>
    public class MoveWrapper
    {
        // ReSharper disable once InconsistentNaming
        protected IMove _move;
        public override string ToString()
        {
            return $"{base.ToString()}+{_move}";
        }
    }

    /// <summary>
    /// いくつかのIMove実装クラスの基底クラス
    /// _movesプロパティを持ち、複数のIMoveをラップできる想定
    /// ToStringをオーバーライドしていて、ネスト関係が見やすいようになっている
    /// </summary>
    public class MovesWrapper
    {
        // ReSharper disable once InconsistentNaming
        protected IMove[] _moves;
        public override string ToString()
        {
            return $"{base.ToString()}+[{string.Join(", ", _moves.Select(m => m.ToString()))}]";
        }
    }

    /// <summary>
    /// ふらふら歩く動き
    /// 引数を指定しない想定
    /// 引数を指定すると、_moveの動きになるが、あまり使用する意味はない
    /// ただし、実装の関係上、Vector.zeroに対応できない
    /// </summary>
    public class WanderingMove : MoveWrapper, IMove, IDisposable
    {
        public struct Context
        {
            public float InputSimulationFrequency;
        }

        private readonly Context _context;
        private Vector3 _simulatedInput;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public WanderingMove(Context context, IMove move)
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
                await UniTask.Delay(TimeSpan.FromSeconds(_context.InputSimulationFrequency),
                    cancellationToken: cancellationToken);
                _simulatedInput = MoveUtility.SimulateRandomInput();
                // Debug.Log(_simulatedInput);
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
        }
    }

    /// <summary>
    /// Targetに対象のTransformをセットすることでそちらに向かう動き
    /// 動き方はmoveで指定可
    /// </summary>
    public class ToTargetMove : MoveWrapper, ITargetMove
    {
        public struct Context
        {
            [NotNull] public Transform Transform;
            public Transform Target;
            public Func<Vector3> GetOffset;
        }

        private readonly Context _context;

        public Transform Target { get; set; }

        public ToTargetMove(Context context, IMove move)
        {
            _context = context;
            _context.GetOffset ??= () => Vector3.zero;
            Target = _context.Target;
            _move = move;
        }

        public void Move(Vector3 input = default)
        {
            if (Target != null)
            {
                input = Utility.SetYToZero(Target.position - _context.Transform.position + _context.GetOffset())
                    .normalized;
            }

            _move.Move(input);
        }
    }

    /// <summary>
    /// Transform.forwardに進み続ける動き
    ///
    /// 実装としては、ToTargetMoveを使用している
    /// 自身とターゲットを同じTransformにして、GetOffsetにtransform.forwardを指定している
    /// </summary>
    public class ToAheadMove : MoveWrapper, IMove
    {
        public ToAheadMove(Transform transform, IMove move)
        {
            _move = new ToTargetMove(new ToTargetMove.Context()
            {
                Transform = transform,
                Target = transform,
                GetOffset = () => transform.forward
            }, move);
        }

        public void Move(Vector3 input = default)
        {
            _move.Move(input);
        }
    }

    /// <summary>
    /// rigidBodyで動き、transformで回転
    ///
    /// </summary>
    public class SimpleMove : MoveWrapper, IMove
    {
        public struct Context
        {
            public GameObject GameObject;
            public float Acceleration;
            public float MaxVelocity;
        }

        public SimpleMove(Context context)
        {
            var transform = context.GameObject.transform;
            _move = new CombinationMove(
                new LookAtMove(transform),
                new ToAheadMove(
                    transform,
                    new AddForceMove(new AddForceMove.Context()
                    {
                        Rb = context.GameObject.GetComponent<Rigidbody>(),
                        Acceleration = context.Acceleration,
                        MaxVelocity = context.MaxVelocity
                    })
                )
            );
        }

        public void Move(Vector3 input = default)
        {
            if (input == Vector3.zero) return;
            _move.Move(input);
        }
    }


    /// <summary>
    /// Transform.LookAtのラッパー
    /// </summary>
    public class LookAtMove : IMove
    {
        private readonly Transform _transform;

        public LookAtMove(Transform transform)
        {
            _transform = transform;
        }

        public void Move(Vector3 input = default)
        {
            var dirToGo = input + _transform.position;
            _transform.LookAt(dirToGo);
        }
    }

    /// <summary>
    /// RigidBodyで動く
    /// Jumpと組み合わさることを考えて、スピード制限はyを除いて行う
    /// </summary>
    public class AddForceMove : IMove
    {
        public struct Context
        {
            public Rigidbody Rb;
            public float Acceleration;
            public float MaxVelocity;
        }

        private readonly Context _context;

        public AddForceMove(Context context)
        {
            _context = context;
        }

        public void Move(Vector3 input = default)
        {
            _context.Rb.AddForce(input * _context.Acceleration, ForceMode.Acceleration);

            var velocity = _context.Rb.velocity;
            velocity.y = 0;
            if (velocity.magnitude >= _context.MaxVelocity)
            {
                velocity = _context.MaxVelocity * velocity.normalized;
                velocity.y = _context.Rb.velocity.y;
                _context.Rb.velocity = velocity;
            }
        }
    }


    /// <summary>
    /// 複数のMoveを一括管理する。
    /// 処理される順番はコンストラクタで追加した順
    /// </summary>
    public class CombinationMove : MovesWrapper, IMove
    {
        public CombinationMove(params IMove[] moves)
        {
            _moves = moves;
        }

        public void Move(Vector3 input = default)
        {
            foreach (var move in _moves)
            {
                move.Move(input);
            }
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