using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Projects.BattleSystem.Enemy.Monster.Interfaces;
using Projects.BattleSystem.Enemy.Scripts;

#nullable  enable

namespace Projects.BattleSystem.Boss.Scripts
{
    public class Monster1Context : IMonster1Context
    {
        public IMonster1State CurrentState { get; private set; }

        public Monster1Context(IMonster1State initState)
        {
            CurrentState = initState;
        }

        public void ChangeState(IMonster1State state)
        {
            CurrentState = state;
        }
    }

    /// <summary>
    ///     各IBoss1Stateの抽象クラス
    /// </summary>
    public abstract class Monster1AbstractState : IMonster1State
    {
        public IEnemyMove EnemyMove => MonsterMove;
        public IEnemyAction EnemyAction => MonsterAction;
        public bool IsActionCompleted => MonsterAction.IsActionCompleted;
        protected Monster1Record Record { get; }
        protected IEnemyAction MonsterAction;
        protected IEnemyMove MonsterMove;
        protected IEnemySearch MonsterSearch;

        public float ActionCoolTime => MonsterAction?.ActionCoolTime ?? 0;

#pragma warning disable CS8618
        protected Monster1AbstractState(Monster1Record record)
#pragma warning restore CS8618
        {
            Record = record;
        }

        public void Move()
        {
            MonsterMove?.Move();
        }
        
        public void StartAction()
        {
            MonsterAction?.StartAction();
        }

        public void EndAction()
        {
            MonsterAction?.EndAction();
        }
        
        public Transform[]? Search()
        {
            return MonsterSearch?.Search();
        }
        
        public Transform? DetermineTarget(IEnumerable<Transform> targetUnits)
        {
            return MonsterSearch?.DetermineTarget(targetUnits);
        }
    }
    
    public class IdleState : Monster1AbstractState
    {
        readonly WanderState? _wanderState;
        public IdleState(Monster1Record record) : base(record)
        {
            MonsterMove = new DoNothingMove();

            MonsterAction = new IdleAction();

            MonsterSearch = new DoNothingSearch();
        }
    }

    public class WanderState : Monster1AbstractState
    {
        public WanderState(Monster1Record record) : base(record)
        {
            MonsterMove =
                new RandomMove(simulationInterval: 2f,
                    new NonTorqueRegularMove(
                        record.Transform, record.Rb, acceleration: 20f, maxVelocity: 1.0f));

            MonsterAction = new DoNothingAction();

            MonsterSearch = new NearestSearch(record.Transform, record.SearchRadius, LayerMask.GetMask("Player"));
        }
    }

    public class TackleState : Monster1AbstractState
    {
        public TackleState(Monster1Record record) : base(record)
        {
            MonsterMove = new TargetMove(record.Transform,
                new NonTorqueRegularMove(
                record.Transform, record.Rb, acceleration: 30f, maxVelocity: 2.5f));
            
            MonsterAction = new TackleAction(record.Transform);
            
            MonsterSearch = new NearestSearch(record.Transform,record.SearchRadius, LayerMask.GetMask("Player"));
        }
        
    }

    public class ChargeJumpState : Monster1AbstractState
    {
        bool _isCharging;
        bool _isCompleted;

        public ChargeJumpState(Monster1Record record) : base(record)
        {
            MonsterMove = new TargetMove(record.Transform,new LookAtInputMoveDecorator(record.Transform, new DoNothingInputMove()));
            
            MonsterAction = new ChargeJumpAction();
            
            MonsterSearch = new  NearestSearch(record.Transform,record.SearchRadius, LayerMask.GetMask("Player"));
        }
        
    }

    public class JumpState : Monster1AbstractState
    {
        bool _isJumping;
        bool _isCompleted;

        public JumpState(Monster1Record record) : base(record)
        {
            MonsterMove = new TargetMove(record.Transform,
                new NonTorqueRegularMove(
                    record.Transform, record.Rb, acceleration: 30f, maxVelocity: 2.5f));

            MonsterAction = new JumpAction(record.Transform,record.Rb);
            
            MonsterSearch = new  NearestSearch(record.Transform,record.SearchRadius, LayerMask.GetMask("Player"));
        }
        
    }

    public class SpitOutState : Monster1AbstractState
    {
        public SpitOutState(Monster1Record record, NetworkRunner runner) : base(record)
        {
            MonsterMove = new TargetMove(record.Transform,new LookAtInputMoveDecorator(record.Transform, new DoNothingInputMove()));
            
            MonsterAction = new SpitOutAction(runner,record.Transform);
            
            MonsterSearch = new FarthestSearch(record.Transform, record.SearchRadius, LayerMask.GetMask("Player"));
        }
        
    }

    public class VacuumState : Monster1AbstractState
    {
        public VacuumState(Monster1Record record) : base(record)
        {
            MonsterMove = new TargetMove(record.Transform, new LookAtInputMoveDecorator(record.Transform, new DoNothingInputMove()));

            MonsterAction = new VacuumAction();
            
            MonsterSearch = new NearestSearch(record.Transform, record.SearchRadius, LayerMask.GetMask("Player"));
        }
        
    }
}