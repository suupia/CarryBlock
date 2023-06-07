using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Main;
using UnityEngine;

namespace Nuts.BattleSystem.Move.Scripts
{
    public interface IMove
    {
        void Move(Vector3 input);
    }

    public interface IBoss1Move
    {
        void Move(Vector3 input = default);
    }

    public interface ITargetMove : IBoss1Move
    {
        Transform Target { get; set; }
    }

    public interface ICancelMove
    {
        void CancelMove();
    }

    /// <summary>
    /// いくつかのIMove実装クラスの基底クラス
    /// _moveプロパティを持ち、
    /// ToStringをオーバーライドしていて、ネスト関係が見やすいようになっている
    /// </summary>
    public class MoveWrapper
    {
        // ReSharper disable once InconsistentNaming
        protected IBoss1Move _move;
        public override string ToString()
        {
            return $"({base.ToString()}+{_move})";
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
        protected IBoss1Move[] _moves;
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
    public class WanderingMove : MoveWrapper, IBoss1Move, ICancelMove
    {
        public record Record
        {
            public float InputSimulationFrequency;
        }

        readonly Record _record;
        private Vector3 _simulatedInput;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public WanderingMove(Record record, IBoss1Move move)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _move = move;
            _record = record;

            StartSimulation(_cancellationTokenSource.Token).Forget();
        }

        public void Move(Vector3 input = default)
        {
            Debug.Log($"WanderingMove -> {input}");
            Debug.Log($"_simulatedInput = {_simulatedInput}");
            Debug.Log($"input == default = {input == default}");
            _move.Move(input == default ? _simulatedInput : input);
        }

        private async UniTaskVoid StartSimulation(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_record.InputSimulationFrequency),
                    cancellationToken: cancellationToken);
                _simulatedInput = MoveUtility.SimulateRandomInput();
                // Debug.Log(_simulatedInput);
            }
        }

        public void CancelMove()
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
        public record Record
        {
            [NotNull] public Transform Transform;
            public Transform Target;
            public Func<Vector3> GetOffset;
        }

        readonly Record _record;

        public Transform Target { get; set; }

        public ToTargetMove(Record record, IBoss1Move move)
        {
            _record = record;
            _record.GetOffset ??= () => Vector3.zero;
            Target = _record.Target;
            _move = move;
        }

        public void Move(Vector3 input = default)
        {
            Debug.Log($"ToTargetMove Transform.name{_record.Transform.name} -> {Target?.name}");
            if (Target != null)
            {
                input = Utility.SetYToZero(Target.position - _record.Transform.position + _record.GetOffset())
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
    public class ToAheadMove : MoveWrapper, IBoss1Move
    {
        public ToAheadMove(Transform transform, IBoss1Move move)
        {
            _move = new ToTargetMove(new ToTargetMove.Record
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
    public class SimpleMove : MoveWrapper, IBoss1Move
    {
        public record Record
        {
            public GameObject GameObject;
            public float Acceleration;
            public float MaxVelocity;
        }

        public SimpleMove(Record record)
        {
            var transform = record.GameObject.transform;
            _move = new CombinationMove(
                new LookAtMove(transform),
                new ToAheadMove(
                    transform,
                    new AddForceMove(new AddForceMove.Record
                    {
                        Rb = record.GameObject.GetComponent<Rigidbody>(),
                        Acceleration = record.Acceleration,
                        MaxVelocity = record.MaxVelocity
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
    /// Moveの引数の方向を向く
    /// </summary>
    public class LookAtMove : IBoss1Move
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
    /// Transform.LookAtのラッパー
    /// </summary>
    public class LookAtTargetMove : ITargetMove
    {
        private readonly Transform _transform;
        public Transform Target { get; set; }

        public LookAtTargetMove(Transform transform)
        {
            _transform = transform;
        }

        public void Move(Vector3 input = default)
        {
            _transform.LookAt(Target);
        }

    }

    /// <summary>
    /// RigidBodyで動く
    /// Jumpと組み合わさることを考えて、スピード制限はyを除いて行う
    /// </summary>
    public class AddForceMove : IBoss1Move
    {
        public record Record
        {
            public Rigidbody Rb;
            public float Acceleration;
            public float MaxVelocity;
        }

        readonly Record _record;

        public AddForceMove(Record record)
        {
            _record = record;
        }

        public void Move(Vector3 input = default)
        {
            _record.Rb.AddForce(input * _record.Acceleration, ForceMode.Acceleration);

            var velocity = Utility.SetYToZero(_record.Rb.velocity);
            if (velocity.magnitude >= _record.MaxVelocity)
            {
                velocity = _record.MaxVelocity * velocity.normalized;
                velocity.y = _record.Rb.velocity.y;
                _record.Rb.velocity = velocity;
            }
        }
    }


    /// <summary>
    /// 複数のMoveを一括管理する。
    /// 処理される順番はコンストラクタで追加した順
    /// </summary>
    public class CombinationMove : MovesWrapper, IBoss1Move
    {
        public CombinationMove(params IBoss1Move[] moves)
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