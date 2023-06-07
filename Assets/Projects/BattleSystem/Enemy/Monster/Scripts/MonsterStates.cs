using System.Collections.Generic;
using Fusion;
using Nuts.BattleSystem.Enemy.Scripts;
using UnityEngine;
using  Nuts.BattleSystem.Enemy.Monster.Interfaces;

namespace Nuts.BattleSystem.Boss.Scripts
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
        public IEnemyMove EnemyMove => move;
        public IEnemyAction EnemyAction => action;
        public bool IsActionCompleted => action.IsActionCompleted;
        Monster1Record Record { get; }
        protected IEnemyAction action;
        protected IEnemyMove move;
        protected IEnemySearch search;
        
        public float ActionCoolTime => action.ActionCoolTime;

        protected Monster1AbstractState(Monster1Record record)
        {
            Record = record;
        }

        public void Move()
        {
            move.Move();
        }
        
        public void StartAction()
        {
            action.StartAction();
        }

        public void EndAction()
        {
            action.EndAction();
        }
        
        public Transform[]? Search()
        {
            return search.Search();
        }
        
        public Transform? DetermineTarget(IEnumerable<Transform> targetUnits)
        {
            return search.DetermineTarget(targetUnits);
        }
    }
    
    public class IdleState : Monster1AbstractState
    {
        readonly WanderState _wanderState;
        public IdleState(Monster1Record record) : base(record)
        {
            move = new DoNothingMove();

            action = new IdleAction();

            search = new DoNothingSearch();
        }
    }

    public class WanderState : Monster1AbstractState
    {
        public WanderState(Monster1Record record) : base(record)
        {
            move =
                new RandomMove(simulationInterval: 2f,
                    new NonTorqueRegularMove(
                        record.Transform, record.Rb, acceleration: 20f, maxVelocity: 1.0f));

            action = new DoNothingAction();

            search = new NearestSearch(record.Transform, record.SearchRadius, LayerMask.GetMask("Player"));
        }
    }

    public class TackleState : Monster1AbstractState
    {
        public TackleState(Monster1Record record) : base(record)
        {
            move = new TargetMove(record.Transform,
                new NonTorqueRegularMove(
                record.Transform, record.Rb, acceleration: 30f, maxVelocity: 2.5f));
            
            action = new TackleAction(record.Transform);
            
            search = new NearestSearch(record.Transform,record.SearchRadius, LayerMask.GetMask("Player"));
        }
        
    }

    public class ChargeJumpState : Monster1AbstractState
    {
        bool _isCharging;
        bool _isCompleted;

        public ChargeJumpState(Monster1Record record) : base(record)
        {
            move = new TargetMove(record.Transform,new LookAtInputMoveDecorator(record.Transform, new DoNothingInputMove()));
            
            action = new ChargeJumpAction();
            
            search = new  NearestSearch(record.Transform,record.SearchRadius, LayerMask.GetMask("Player"));
        }
        
    }

    public class JumpState : Monster1AbstractState
    {
        bool _isJumping;
        bool _isCompleted;

        public JumpState(Monster1Record record) : base(record)
        {
            move = new TargetMove(record.Transform,
                new NonTorqueRegularMove(
                    record.Transform, record.Rb, acceleration: 30f, maxVelocity: 2.5f));

            action = new JumpAction(record.Transform,record.Rb);
            
            search = new  NearestSearch(record.Transform,record.SearchRadius, LayerMask.GetMask("Player"));
        }
        
    }

    public class SpitOutState : Monster1AbstractState
    {
        public SpitOutState(Monster1Record record, NetworkRunner runner) : base(record)
        {
            move = new TargetMove(record.Transform,new LookAtInputMoveDecorator(record.Transform, new DoNothingInputMove()));
            
            action = new SpitOutAction(runner,record.Transform);
            
            search = new FarthestSearch(record.Transform, record.SearchRadius, LayerMask.GetMask("Player"));
        }
        
    }

    public class VacuumState : Monster1AbstractState
    {
        public VacuumState(Monster1Record record) : base(record)
        {
            move = new TargetMove(record.Transform, new LookAtInputMoveDecorator(record.Transform, new DoNothingInputMove()));

            action = new VacuumAction();
            
            search = new NearestSearch(record.Transform, record.SearchRadius, LayerMask.GetMask("Player"));
        }
        
    }
}