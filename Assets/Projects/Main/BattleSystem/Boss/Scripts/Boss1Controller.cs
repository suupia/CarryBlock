using System;
using System.Collections.Generic;
using Animations;
using Decoration;
using Fusion;
using Main;
using UnityEngine;

namespace Boss
{
    public class Boss1Controller : NetworkBehaviour
    {
        enum State
        {
            None,
            Lost,
            Detected
        }

        [SerializeField] private GameObject modelObject;

        [Networked] ref Boss1DecorationDetector.Data DecorationDataRef => ref MakeRef<Boss1DecorationDetector.Data>();

        [Networked] private TickTimer AttackTimer { get; set; }

        [Networked] private int Hp { get; set; } = 1;

        private IMove DefaultMove => new SimpleMove(gameObject);
        private string DebugText => $"State: {_state}";

        private bool IsLostPlayers => _colliders.Length == 0;

        //ステートパターンに従う
        //インスタンスを使用する方法でも良いが、一旦列挙体で管理する。
        private State _state = State.None;

        private IMove _move;
        private ISearch _search;
        private IAttack _attack; 
        
        private Boss1DecorationDetector _decorationDetector;
        private readonly HashSet<Transform> _targetBuffer = new();
        private Collider[] _colliders;

        public override void Spawned()
        {
            SetUp();
        }

        void SetUp()
        {
            _decorationDetector = new Boss1DecorationDetector(new Boss1AnimatorSetter(modelObject));
            
            SetMove(new WanderingMove(DefaultMove));
            _search = new RangeSearch(transform, 6, LayerMask.GetMask("Player"));
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;

            //Move
            if (_attack is ITargetAttack attack && attack.Target != null)
            {
                _move.Move(attack.Target.position);
            }
            else
            {
                _move.Move();
            }

            if (AttackTimer.ExpiredOrNotRunning(Runner))
            {
                //Search
                _colliders = _search.Search();

                if (IsLostPlayers)
                {
                    SetState(State.Lost);
                }
                else
                {
                    SetState(State.Detected);
                    
                    _targetBuffer.Clear();
                    _targetBuffer.UnionWith(_colliders.Map(c => c.transform));

                    //Attack
                    _attack.Attack();

                    //SetTimer
                    //攻撃時のクールタイムを設定する
                    AttackTimer = TickTimer.CreateFromSeconds(Runner, 3f);
                }
            }
        }
        
        //Do not call in Render loop
        //HostのFixedUpdateNetworkでのみ呼び出す想定
        void SetState(State state)
        {
            if (_state == state) return;
            _state = state;

            
            switch (state)
            {
                case State.None:
                    break;
                case State.Lost:
                    _attack = null;
                    _decorationDetector.OnEndTackle(ref DecorationDataRef);
                    SetMove(new WanderingMove(DefaultMove));
                    break;
                case State.Detected:
                    _attack = new ToNearestAttack(transform, _targetBuffer, new ToTargetAttack(gameObject));
                    _decorationDetector.OnStartTackle(ref DecorationDataRef);
                    SetMove(new ToTargetMove(transform, DefaultMove));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

        }

        void SetMove(IMove move)
        {
            if (_move is IDisposable preMove)
            {
                preMove.Dispose();
            }

            _move = move;
        }

        public override void Render()
        {
            _decorationDetector.OnRendered(ref DecorationDataRef, Hp);
        }
    }
}