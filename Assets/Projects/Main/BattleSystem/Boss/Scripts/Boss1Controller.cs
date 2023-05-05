using System;
using System.Collections.Generic;
using System.Linq;
using Animations;
using Decoration;
using Fusion;
using Main;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;
using Random = UnityEngine.Random;

namespace Boss
{
    public class Boss1Controller : NetworkBehaviour
    {
        private enum State
        {
            None,
            Lost,
            Detected,
            Jumping,
            ChargingJump,
            SpitOut,
        }

        [SerializeField] private GameObject modelObject;
        [SerializeField] private State overrideState;
        [SerializeField] private Transform finSpawnerTransform;

        private const float JumpTime = 2f;
        private const float ChargeJumpTime = 0.5f;
        private const float ChargeSpitOutTime = 1.5f;
        private const float SearchRadius = 6f;

        [Networked]
        private ref Boss1DecorationDetector.Data DecorationDataRef => ref MakeRef<Boss1DecorationDetector.Data>();

        //Timer
        [Networked] private TickTimer AttackTimer { get; set; }
        [Networked] private TickTimer SetAsWillStateTimer { get; set; }

        [Networked] private int Hp { get; set; } = 1;

        private bool IsLostPlayers => _colliders.Length == 0;

        //ステートパターンに従う
        //インスタンスを使用する方法でも良いが、一旦列挙体で管理する。
        private State _state = State.None;

        private IMove _move;
        private ISearch _search;
        private IAttack _attack;

        private Boss1DecorationDetector _decorationDetector;

        private readonly HashSet<Transform> _targetBuffer = new();
        private Collider[] _colliders = { };
        private Rigidbody _rd;
        private State _willState;


        //TODO: より良い管理方法を考える
        //Trigger的なDecorationはAttackが呼ばれるたびに呼んでほしい
        Action _onAttack = () => { };

        //Define Template Moves 
        private IMove DefaultMove => new SimpleMove(new SimpleMove.Context()
        {
            GameObject = gameObject,
            Acceleration = 20f,
            MaxVelocity = 1f
        });

        private IMove SpeedyMove => new SimpleMove(new SimpleMove.Context()
        {
            GameObject = gameObject,
            Acceleration = 30f,
            MaxVelocity = 2.5f
        });

        private IMove WanderingMove => new WanderingMove(
            context: new WanderingMove.Context()
            {
                InputSimulationFrequency = 2f
            },
            move: DefaultMove
        );

        private IMove ToTargetMove =>
            new ToTargetMove(
                new ToTargetMove.Context()
                {
                    Transform = transform
                }, SpeedyMove);

        private IMove LookAtTargetMove => new LookAtTargetMove(transform);

        private string DebugText => $"State: {_state}\nMove: {_move}\nAttack: {_attack}";

        private void Start()
        {
            _rd = GetComponent<Rigidbody>();
        }

        public override void Spawned()
        {
            SetUp();
        }

        private void SetUp()
        {
            _decorationDetector = new Boss1DecorationDetector(new Boss1AnimatorSetter(modelObject));
            _attack = null;
            _move = WanderingMove;
            _search = new RangeSearch(transform: transform, radius: SearchRadius,
                layerMask: LayerMask.GetMask("Player"));
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;

            Move();

            CheckWillStateTimer();

            CheckAttackTimer();
        }

        private void CheckAttackTimer()
        {
            if (AttackTimer.ExpiredOrNotRunning(Runner))
            {
                //Search
                //現状は攻撃時にサーチする
                _colliders = _search.Search();

                //Set Detected Targets
                //このバッファを更新することで、ToNearestAttackが機能する
                _targetBuffer.Clear();
                _targetBuffer.UnionWith(_colliders.Map(c => c.transform));

                if (IsLostPlayers)
                {
                    SetState(State.Lost);
                }
                else
                {
                    //前の状態がLostなら、新しい攻撃状態に入る
                    if (_state == State.Lost)
                    {
                        //攻撃手法の抽出方法はまだ未検討
                        var state = ChooseState();
                        SetState(state);
                    }

                    //Attack
                    _attack?.Attack();
                    _onAttack(); //Assume calling Decoration callback

                    //SetTimer
                    //攻撃時のクールタイムを設定する
                    AttackTimer = TickTimer.CreateFromSeconds(Runner, 4f);
                }
            }
        }

        private State ChooseState()
        {
            if (overrideState != State.None)
            {
                return overrideState;
            }

            var detectedStates = new[] { State.ChargingJump, State.Detected, State.SpitOut };
            var state = detectedStates[Random.Range(0, detectedStates.Length)];
            return state;
        }

        private void CheckWillStateTimer()
        {
            if (SetAsWillStateTimer.Expired(Runner))
            {
                SetAsWillStateTimer = TickTimer.None;
                SetState(_willState);
            }
        }

        private void Move()
        {
            //Set Target if ITargetMove
            if (_move is ITargetMove move)
            {
                if (_attack is ITargetAttack attack)
                {
                    move.Target = attack.Target;
                }
            }

            //Move
            _move?.Move();
        }

        //Do not call in Client loop
        //HostのFixedUpdateNetworkでのみ呼び出す想定
        private void SetState(State state)
        {
            if (_state == state) return;

            var preState = _state;
            _state = state;

            //新しい状態がJumpingのとき、前の状態はChargingJumpである
            Assert.IsTrue(state != State.Jumping || preState == State.ChargingJump);

            // Debug.Log($"State was changed to {state}");

            if (_move is IDisposable move)
            {
                move.Dispose();
            }

            switch (state)
            {
                case State.None:
                    break;
                case State.Lost:
                    _attack = null;
                    switch (preState)
                    {
                        case State.Detected:
                            _decorationDetector.OnEndTackle(ref DecorationDataRef);
                            break;
                        case State.Jumping:
                            _decorationDetector.OnEndJump(ref DecorationDataRef);
                            break;
                    }

                    _onAttack = () => { };
                    _move = WanderingMove; //ふらつく動き
                    break;
                case State.Detected:
                    _attack = new ToNearestAttack(
                        new TargetBufferAttack.Context()
                        {
                            Transform = transform,
                            TargetBuffer = _targetBuffer
                        },
                        new ToTargetAttack(
                            gameObject,
                            new MockAttack()
                        )
                    );
                    _decorationDetector.OnStartTackle(ref DecorationDataRef);
                    _move = ToTargetMove; //ターゲットに向かう動き
                    break;
                case State.Jumping:
                    _move = ToTargetMove;
                    MoveUtility.Jump(_rd, JumpTime); //ジャンプのインパルスを与える

                    DelaySetState(State.Lost, JumpTime);
                    break;
                case State.ChargingJump:
                    GameObject o;
                    _attack = new ToNearestAttack(
                        new TargetBufferAttack.Context()
                        {
                            Transform = transform,
                            TargetBuffer = _targetBuffer
                        },
                        new ToTargetAttack(
                            (o = gameObject),
                            new DelayAttack(
                                JumpTime + ChargeJumpTime,
                                new RangeAttack(o, 3)
                            )
                        )
                    );
                    _decorationDetector.OnStartJump(ref DecorationDataRef);
                    _move = null;

                    DelaySetState(State.Jumping, ChargeJumpTime);
                    break;
                case State.SpitOut:

                    _attack = new ToFurthestAttack(
                        new TargetBufferAttack.Context()
                        {
                            Transform = transform,
                            TargetBuffer = _targetBuffer
                        },
                        new ToTargetAttack(
                            gameObject,
                            new DelayAttack(
                                ChargeSpitOutTime,
                                new LaunchNetworkObjectAttack(
                                    new LaunchNetworkObjectAttack.Context()
                                    {
                                        Runner = Runner,
                                        From = finSpawnerTransform,
                                        Prefab = Resources.Load<GameObject>("Prefabs/Attacks/Fin"),
                                    }
                                )
                            )
                        )
                    );
                    _onAttack = () => { _decorationDetector.OnSpitOut(ref DecorationDataRef); };
                    _move = LookAtTargetMove;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }


        //TickTimerを用いて、状態を遅延セット
        private void DelaySetState(State state, float delay)
        {
            // Debug.Log($"Delay set called to {state}");
            SetAsWillStateTimer = TickTimer.CreateFromSeconds(Runner, delay);
            _willState = state;
        }

        public override void Render()
        {
            _decorationDetector.OnRendered(DecorationDataRef, Hp);
        }


        private void OnGUI()
        {
            // ラベルを表示
            GUI.Label(new Rect(10, 10, 600, 150), DebugText);

            // // ボタンを表示
            // if (GUI.Button(new Rect(10, 40, 100, 20), "Click me"))
            // {
            //     _message = "Button clicked!";
            // }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, SearchRadius);
        }
    }
}