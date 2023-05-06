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
    public partial class Boss1Controller : NetworkBehaviour
    {
        private enum State
        {
            None,
            Lost,
            Tackling,
            Jumping,
            ChargingJump,
            SpitOut,
            Vacuuming,
        }

        struct StateContext
        {
            public IMove Move;
            public ISearch Search;
            public IAttack Attack;
        }

        [SerializeField] private GameObject modelObject;
        [SerializeField] private Transform finSpawnerTransform;
        [SerializeField] private State overrideOnDetectedState;
        [SerializeField] private bool showGizmos;
        [SerializeField] private bool showGUI;

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
        private StateContext _context;

        private Boss1DecorationDetector _decorationDetector;

        private readonly HashSet<Transform> _targetBuffer = new();
        private Collider[] _colliders = { };
        private Rigidbody _rd;
        private State _willState;


        //TODO: より良い管理方法を考える
        //Trigger的なDecorationはAttackが呼ばれるたびに呼んでほしい
        private Action _onAttack = () => { };

        private string DebugText => $"State: {_state}\nMove: {_context.Move}\nAttack: {_context.Attack}";

        public override void Spawned()
        {
            SetUp();
        }

        private void SetUp()
        {
            _rd = GetComponent<Rigidbody>();
            _decorationDetector = new Boss1DecorationDetector(new Boss1AnimatorSetter(modelObject));

            if (HasStateAuthority)
            {
                SetState(State.Lost);
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;

            Move();

            CheckWillStateTimer();

            CheckAttackTimer();
        }

        private void Move()
        {
            //Set Target if ITargetMove
            if (_context.Move is ITargetMove move)
            {
                if (_context.Attack is ITargetAttack attack)
                {
                    move.Target = attack.Target;
                }
            }

            //Move
            _context.Move?.Move();
        }

        private State ChooseState()
        {
            var state = overrideOnDetectedState != State.None
                ? overrideOnDetectedState
                : _detectedStates[Random.Range(0, _detectedStates.Length)];
            return state;
        }

        private void CheckAttackTimer()
        {
            if (AttackTimer.ExpiredOrNotRunning(Runner))
            {
                Search();

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
                    _context.Attack?.Attack();
                    _onAttack(); //Assume calling Decoration callback

                    //SetTimer
                    //攻撃時のクールタイムを設定する
                    AttackTimer = TickTimer.CreateFromSeconds(Runner, 4f);
                }
            }
        }

        private void Search()
        {
            //Search
            //現状は攻撃時にサーチする
            _colliders = _context.Search.Search();

            //Set Detected Targets
            //このバッファを更新することで、ToNearestAttackが機能する
            _targetBuffer.Clear();
            _targetBuffer.UnionWith(_colliders.Map(c => c.transform));
        }

        private void CheckWillStateTimer()
        {
            if (SetAsWillStateTimer.Expired(Runner))
            {
                SetAsWillStateTimer = TickTimer.None;
                SetState(_willState);
            }
        }
        
        //Do not call in Client loop
        //HostのFixedUpdateNetworkでのみ呼び出す想定
        private void SetState(State state)
        {
            if (_state == state) return;
            var preState = _state;

            //新しい状態がJumpingのとき、前の状態はChargingJumpである
            Assert.IsTrue(state != State.Jumping || preState == State.ChargingJump);
            //Noneには遷移しない。Noneはクライアント用の状態
            Assert.IsFalse(state == State.None);
            
            if (_context.Move is IDisposable move)
            {
                move.Dispose();
            }

            _state = state;
            _context = _get[_state]();


            switch (state)
            {
                case State.Lost:
                    switch (preState)
                    {
                        case State.Tackling:
                            _decorationDetector.OnEndTackle(ref DecorationDataRef);
                            break;
                        case State.Jumping:
                            _decorationDetector.OnEndJump(ref DecorationDataRef);
                            break;
                        case State.Vacuuming:
                            _decorationDetector.OnEndVacuum(ref DecorationDataRef);
                            break;
                    }

                    _onAttack = () => { };
                    break;
                case State.Tackling:
                    _decorationDetector.OnStartTackle(ref DecorationDataRef);
                    break;
                case State.Jumping:
                    MoveUtility.Jump(_rd, JumpTime); //ジャンプのインパルスを与える
                    DelaySetState(State.Lost, JumpTime);
                    break;
                case State.ChargingJump:
                    _decorationDetector.OnStartJump(ref DecorationDataRef);
                    DelaySetState(State.Jumping, ChargeJumpTime);
                    break;
                case State.SpitOut:
                    _onAttack = () => { _decorationDetector.OnSpitOut(ref DecorationDataRef); };
                    break;
                case State.Vacuuming:
                    _decorationDetector.OnStartVacuum(ref DecorationDataRef);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }


        //TickTimerを用いて、状態を遅延セット
        //このクラスのメンバ変数を使うので完璧に動くわけではない
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
            if (!showGUI) return;
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
            if (!showGizmos) return;
            //サーチ範囲を表示
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, SearchRadius);
        }
    }
}