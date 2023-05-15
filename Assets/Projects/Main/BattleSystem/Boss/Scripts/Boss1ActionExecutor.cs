using System.Collections;
using System.Collections.Generic;
using Fusion;
using Main;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

# nullable enable

namespace Boss
{
    /// <summary>
    /// 何もしないIEnemyAttackExecutorの実装クラス
    /// </summary>
    public class DoNothingAction : IEnemyActionExecutor
    {
        public float ActionCoolTime => 0;

        public DoNothingAction()
        {
        }

        public void StartAction() 
        {
        }
        
        public void EndAction()
        {
        }
    }
    public class IdleAction : IEnemyActionExecutor
    {
        public float ActionCoolTime => 0.6f;
        bool _isDelaying;
        bool _isCompleted;
        CancellationTokenSource _cts;
        public float delayTime { get; init; } = 0.3f;
        readonly IBoss1State _nextState;
        readonly IBoss1Context _context;

        public IdleAction(IBoss1State nextState, IBoss1Context context)
        {
            _nextState = nextState;
            _context = context;
            _cts = new CancellationTokenSource();
        }
        
        
        public void StartAction()
        {
            Debug.Log($"IdleAction StartAction _isCompleted:{_isCompleted}");
            if (_isCompleted)
            {
                Reset();
                _context.ChangeState(_nextState);
            }
            else
            {
                Delay().Forget();
            }
        }

        public void EndAction()
        {
            Reset();
        }

        async UniTaskVoid Delay()
        {
            if (_isDelaying) return;
            _isDelaying = true;
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(delayTime), cancellationToken: _cts.Token);
            }catch(OperationCanceledException)
            {
                Reset();
                return;
            }
            _isDelaying = false;
            _isCompleted = true;
        }

        void Reset()
        {
            _isDelaying = false;
            _isCompleted = false;
            _cts = new CancellationTokenSource();
        }
    }

    public class TackleAction : IEnemyActionExecutor
    {
        public float ActionCoolTime { get; init; } = 1.0f;
        ComponentPrefabInstantiate<AttackCollider> _attackColliderInstantiate;
        readonly AttackCollider _attackCollider;
        readonly string _prefabName = "TackleAttackCollider";

        public TackleAction(Transform parent)
        {
            _attackColliderInstantiate = new(
                new PrefabLoaderFromResources<AttackCollider>("Prefabs/Attacks"), 
                "TackleCollider"); // ToDo: _prefabNameを代入する
            _attackCollider = _attackColliderInstantiate.InstantiatePrefab(parent);
        }
        
        
        public void StartAction()
        {
            _attackCollider.gameObject.SetActive(true);
        }

        public void EndAction()
        {
            _attackCollider.gameObject.SetActive(false);
        }
    }
    
    public class ChargeJumpAction : IEnemyActionExecutor
    {
        public float ActionCoolTime => 0;
        bool _isCharging;
        bool _isCompleted;
        public float chargeTime { get; init; } = 0.5f;
        readonly IBoss1State _nextState;
        readonly IBoss1Context _context;
        readonly CancellationTokenSource _cts;

        public ChargeJumpAction(IBoss1State nextState, IBoss1Context context)
        {
            _nextState = nextState;
            _context = context;
            _cts = new CancellationTokenSource();
        }
        
        
        public void StartAction()
        {
            if (_isCompleted)
            {
                Reset();
                _context.ChangeState(_nextState);
            }
            else
            {
                ChargeJump().Forget();
            }
        }

        public void EndAction()
        {
            Reset();
        }

        async UniTaskVoid ChargeJump()
        {
            if (_isCharging) return;
            _isCharging = true;
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(chargeTime), cancellationToken: _cts.Token);
            }catch(OperationCanceledException)
            {
                Reset();
                return;
            }
            _isCharging = false;
            _isCompleted = true;
        }

        void Reset()
        {
            _isCharging = false;
            _isCompleted = false;
            _cts.Cancel();
        }
    }

    public class JumpAction : IEnemyActionExecutor
    {
        public float ActionCoolTime { get; init; } = 4.0f;
        ComponentPrefabInstantiate<AttackCollider> _attackColliderInstantiate;
        bool _isJumping;
        bool _isCompleted;
        public float jumpTime { get; init; } = 2f;
        readonly AttackCollider _attackCollider;
        readonly string _prefabName = "JumpAttackCollider";
        readonly IBoss1State _nextState;
        readonly IBoss1Context _context;
        readonly Rigidbody _rb;
        readonly CancellationTokenSource _cts;

        public JumpAction(IBoss1State nextState,IBoss1Context context, Transform parent, Rigidbody rb)
        {
            _attackColliderInstantiate = new(
                new PrefabLoaderFromResources<AttackCollider>("Prefabs/Attacks"), 
                "JumpCollider"); // ToDo: _prefabNameを代入する
            _attackCollider = _attackColliderInstantiate.InstantiatePrefab(parent);
            _nextState = nextState;
            _context = context;
            _rb = rb;

            _cts = new CancellationTokenSource();
        }
        
        public void StartAction()
        {
            if (_isCompleted)
            {
                Reset();
                _context.ChangeState(_nextState);
            }
            else
            {
                Jump().Forget();
            }
        }

        public void EndAction()
        {
            Reset();
        }

        async UniTaskVoid Jump()
        {
            if (_isJumping) return;
            _isJumping = true;
            MoveUtility.Jump(_rb,jumpTime);
            
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(jumpTime), cancellationToken: _cts.Token);
            }catch(OperationCanceledException)
            {
                Reset();
                return;
            }
            _isJumping = false;
            _isCompleted = true;
        }

        void Reset()
        {
            _isJumping = false;
            _isCompleted = false;
        }
    }

    public class SpitOutAction : IEnemyTargetActionExecutor
    {
        public Transform Target { get; set; }
        NetworkBehaviourPrefabSpawner<NetworkTargetAttackCollider> _attackColliderSpawner;
        public float ActionCoolTime { get; init; } = 4.0f;
        readonly Transform _transform;
        readonly string _prefabName = "SpitOutAttackCollider";

        public SpitOutAction(NetworkRunner runner, Transform transform)
        {
            _transform = transform;
            _attackColliderSpawner = new(runner,
                new PrefabLoaderFromResources<NetworkTargetAttackCollider>("Prefabs/Attacks"), 
                "SpitOutCollider"); // ToDo: _prefabNameを代入する
        }

        public void StartAction()
        {
           var targetAttack =   _attackColliderSpawner.SpawnPrefab(_transform.position, Quaternion.identity, PlayerRef.None);
           targetAttack.Init(Target);
        }

        public void EndAction()
        {
            // 特になし
        }
    }

    public class VacuumAction : IEnemyTargetActionExecutor
    {
        public Transform? Target { get; set; }
        public float ActionCoolTime { get; init; } = 4.0f;
        public VacuumAction()
        {
            
        }
        
        public void StartAction()
        {
            // ToDo: バキュームの実装をする
        }

        public void EndAction()
        {
            // ToDo: バキュームの終了の実装をする
        }
    }

}