using System;
using System.Collections.Generic;
using Decoration;
using Fusion;
using Main;
using UnityEngine;

namespace Boss
{
    public class BossController : NetworkBehaviour
    {
        enum State
        {
            None,
            Lost,
            Detected
        }

        [SerializeField] private GameObject modelObject;

        [Networked] ref BossDecorationDetector.Data DecorationDataRef => ref MakeRef<BossDecorationDetector.Data>();
        private BossDecorationDetector _decorationDetector;

        [Networked] private TickTimer AttackTimer { get; set; }

        private IMove DefaultMove => new SimpleMove(gameObject);
        private string DebugText => $"State: {_state}";

        private bool IsLostPlayers => _colliders.Length == 0;

        private State _state = State.None;

        private IMove _move;
        private ISearch _search;
        private IAttack _attack;

        private readonly HashSet<Transform> _targetBuffer = new();
        private Collider[] _colliders;

        public override void Spawned()
        {
            SetUp();
        }

        void SetUp()
        {
            OnLostPlayers();
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
        
        void SetState(State state)
        {
            if (_state == state) return;
            _state = state;

            
            switch (state)
            {
                case State.None:
                    break;
                case State.Lost:
                    OnLostPlayers();
                    break;
                case State.Detected:
                    OnDetectedPlayers();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

        }
        
        
        void OnDetectedPlayers()
        {
            _attack = new ToNearestAttack(transform, _targetBuffer, new ToTargetAttack(gameObject));
            SetMove(new ToTargetMove(transform, DefaultMove));
        }

        void OnLostPlayers()
        {
            _attack = null;
            SetMove(new WanderingMove(DefaultMove));
        }

        void SetMove(IMove move)
        {
            if (_move is IDisposable preMove)
            {
                preMove.Dispose();
            }

            _move = move;
        }
    }
}