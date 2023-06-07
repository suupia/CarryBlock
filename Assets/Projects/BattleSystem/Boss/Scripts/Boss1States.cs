using System.Collections.Generic;
using Fusion;
using Nuts.Projects.BattleSystem.Main.BattleSystem.Enemy.Scripts;
using UnityEngine;

namespace Nuts.BattleSystem.Boss.Scripts
{
    
    //Stateパターン
    public interface IBoss1Context
    {
        public IBoss1State CurrentState { get; }
        public void ChangeState(IBoss1State state);
    }

    public interface IBoss1State : IEnemyMoveExecutor, IEnemyActionExecutor, IEnemySearchExecutor
    {
        IEnemyMoveExecutor EnemyMove { get; } 
        IEnemyActionExecutor EnemyAction { get; }
    }


    public class Boss1Context : IBoss1Context
    {
        public IBoss1State CurrentState { get; private set; }

        public Boss1Context(IBoss1State initState)
        {
            CurrentState = initState;
        }

        public void ChangeState(IBoss1State state)
        {
            CurrentState = state;
        }
    }

    /// <summary>
    ///     各IBoss1Stateの抽象クラス
    /// </summary>
    public abstract class Boss1AbstractState : IBoss1State
    {
        public IEnemyMoveExecutor EnemyMove => move;
        public IEnemyActionExecutor EnemyAction => action;
        public bool IsActionCompleted => action.IsActionCompleted;
        Boss1Record Record { get; }
        protected IEnemyActionExecutor action;
        protected IEnemyMoveExecutor move;
        protected IEnemySearchExecutor search;
        
        public float ActionCoolTime => action.ActionCoolTime;

        protected Boss1AbstractState(Boss1Record record)
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
    
    public class IdleState : Boss1AbstractState
    {
        readonly WanderState _wanderState;
        public IdleState(Boss1Record record) : base(record)
        {
            move = new DoNothingMove();

            action = new IdleAction();

            search = new DoNothingSearch();
        }
    }

    public class WanderState : Boss1AbstractState
    {
        public WanderState(Boss1Record record) : base(record)
        {
            move =
                new RandomMove(simulationInterval: 2f,
                    new NonTorqueRegularMove(
                        record.Transform, record.Rb, acceleration: 20f, maxVelocity: 1.0f));

            action = new DoNothingAction();

            search = new NearestSearch(record.Transform, record.SearchRadius, LayerMask.GetMask("Player"));
        }
    }

    public class TackleState : Boss1AbstractState
    {
        public TackleState(Boss1Record record) : base(record)
        {
            move = new TargetMove(record.Transform,
                new NonTorqueRegularMove(
                record.Transform, record.Rb, acceleration: 30f, maxVelocity: 2.5f));
            
            action = new TackleAction(record.Transform);
            
            search = new NearestSearch(record.Transform,record.SearchRadius, LayerMask.GetMask("Player"));
        }
        
    }

    public class ChargeJumpState : Boss1AbstractState
    {
        bool _isCharging;
        bool _isCompleted;

        public ChargeJumpState(Boss1Record record) : base(record)
        {
            move = new TargetMove(record.Transform,new LookAtInputMoveDecorator(record.Transform, new DoNothingInputMove()));
            
            action = new ChargeJumpAction();
            
            search = new  NearestSearch(record.Transform,record.SearchRadius, LayerMask.GetMask("Player"));
        }
        
    }

    public class JumpState : Boss1AbstractState
    {
        bool _isJumping;
        bool _isCompleted;

        public JumpState(Boss1Record record) : base(record)
        {
            move = new TargetMove(record.Transform,
                new NonTorqueRegularMove(
                    record.Transform, record.Rb, acceleration: 30f, maxVelocity: 2.5f));

            action = new JumpAction(record.Transform,record.Rb);
            
            search = new  NearestSearch(record.Transform,record.SearchRadius, LayerMask.GetMask("Player"));
        }
        
    }

    public class SpitOutState : Boss1AbstractState
    {
        public SpitOutState(Boss1Record record, NetworkRunner runner) : base(record)
        {
            move = new TargetMove(record.Transform,new LookAtInputMoveDecorator(record.Transform, new DoNothingInputMove()));
            
            action = new SpitOutAction(runner,record.Transform);
            
            search = new FarthestSearch(record.Transform, record.SearchRadius, LayerMask.GetMask("Player"));
        }
        
    }

    public class VacuumState : Boss1AbstractState
    {
        public VacuumState(Boss1Record record) : base(record)
        {
            move = new TargetMove(record.Transform, new LookAtInputMoveDecorator(record.Transform, new DoNothingInputMove()));

            action = new VacuumAction();
            
            search = new NearestSearch(record.Transform, record.SearchRadius, LayerMask.GetMask("Player"));
        }
        
    }
}