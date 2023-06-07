using System;
using System.Collections.Generic;
using Fusion;
using Main;
using Nuts.Projects.BattleSystem.Decoration.Animations.Scripts;
using Nuts.Projects.BattleSystem.Decoration.Scripts;
using Nuts.Projects.BattleSystem.Main.BattleSystem.Boss_Previous.Attack;
using Nuts.Projects.BattleSystem.Main.BattleSystem.Boss_Previous.Search;
using Nuts.Projects.BattleSystem.Main.BattleSystem.Move;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;
using Random = UnityEngine.Random;

namespace Nuts.Projects.BattleSystem.Main.BattleSystem.Boss_Previous.Controller
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

        class StateContext
        {
            public IBoss1Move Move;
            public IBoss1Search Search;
            public IBoss1Attack Attack;
            public float AttackCoolTime;
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
        
        //Status
        [Networked] private int Hp { get; set; } = 1;
        

        //ステートパターンに従う
        //インスタンスを使用する方法でも良いが、一旦列挙体で管理する。
        private State _state = State.None;
        private StateContext _context;

        private Boss1DecorationDetector _decorationDetector;

        private readonly HashSet<Transform> _targetBuffer = new();
        private Rigidbody _rd;
        private State _willState;


        //TODO: より良い管理方法を考える
        //Trigger的なDecorationはAttackが呼ばれるたびに呼んでほしい
        private Action _onAttack = () => { };

        private string DebugText =>
            $"State: {_state}\nMove: {_context?.Move}\nAttack: {_context?.Attack}" +
            $"\nMove Target: {(_context?.Move as ITargetMove)?.Target}" +
            $"\nAttack Target: {(_context?.Attack as ITargetAttack)?.Target}";

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
            //Set Target if ITargetMove and ITargetAttack
            //以下はパターン構文。MoveとAttackプロパティのタイプチェックをしている
            if (_context is { Move: ITargetMove move, Attack: ITargetAttack attack })
            {
                move.Target = attack.Target;
            }

            //Move
            _context.Move?.Move();
        }

        private State ChooseState()
        {
            var state = overrideOnDetectedState != State.None
                ? overrideOnDetectedState
                : _detectedStates[Random.Range((int)0, (int)_detectedStates.Length)];
            return state;
        }

        private void CheckAttackTimer()
        {
            if (AttackTimer.ExpiredOrNotRunning(Runner))
            {
                var colliders = Search();

                if (IsLostPlayers(colliders))
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
                    // Debug.Log($"Now attack is {_context.Attack}");
                    _context.Attack?.Attack();
                    _onAttack(); //Assume calling Decoration callback

                    //SetTimer
                    //攻撃時のクールタイムを設定する
                    AttackTimer = TickTimer.CreateFromSeconds(Runner, _context.AttackCoolTime);
                }
            }
        }

        Collider[] Search()
        {
            //Search
            //現状は攻撃時にサーチする
            var colliders = _context.Search.Search();

            //Set Detected Targets
            //このバッファを更新することで、ToNearestAttackが機能する
            _targetBuffer.Clear();
            _targetBuffer.UnionWith(colliders.Map(c => c.transform));

            return colliders;
        }

        bool IsLostPlayers(Collider[] colliders)
        {
            return colliders.Length == 0;
        }

        private void CheckWillStateTimer()
        {
            if (SetAsWillStateTimer.Expired(Runner))
            {
                SetAsWillStateTimer = TickTimer.None;
                AttackTimer = TickTimer.None;
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

            if (_context?.Move is ICancelMove move)
            {
                move.CancelMove();
            }

            _state = state;
            _context = _get[_state]();
            
            //Attackがnullでないとき、AttackCoolTimeは必須
            Assert.IsTrue(_context.Attack == null || _context.AttackCoolTime > 0);

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
                    MoveUtility.Jump(_rd, Boss1Controller.JumpTime); //ジャンプのインパルスを与える
                    DelaySetState(State.Lost, Boss1Controller.JumpTime);
                    break;
                case State.ChargingJump:
                    _decorationDetector.OnStartJump(ref DecorationDataRef);
                    DelaySetState(State.Jumping, Boss1Controller.ChargeJumpTime);
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
        //外部スコープの変数を使うので完璧に動くわけではない
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

            // ボタンを表示
            if (GUI.Button(new Rect(600, 10, 120, 20), "Show in console"))
            {
                Debug.Log(DebugText);
            }
        }

        private void OnDrawGizmos()
        {
            if (!showGizmos) return;
            //サーチ範囲を表示
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Boss1Controller.SearchRadius);
        }
    }
}