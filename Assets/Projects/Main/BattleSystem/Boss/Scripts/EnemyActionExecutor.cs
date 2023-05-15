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
        readonly JumpState _jumpState;
        readonly IBoss1Context _context;
        readonly CancellationTokenSource _cts;

        public ChargeJumpAction(JumpState jumpState, IBoss1Context context)
        {
            _jumpState = jumpState;
            _context = context;
            _cts = new CancellationTokenSource();
        }
        
        
        public void StartAction()
        {
            if (_isCompleted)
            {
                Reset();
                _context.ChangeState(_jumpState);
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
        readonly IdleState _idleState;
        readonly IBoss1Context _context;
        readonly Rigidbody _rb;
        readonly CancellationTokenSource _cts;

        public JumpAction(IdleState idleState,IBoss1Context context, Transform parent, Rigidbody rb)
        {
            _attackColliderInstantiate = new(
                new PrefabLoaderFromResources<AttackCollider>("Prefabs/Attacks"), 
                "JumpCollider"); // ToDo: _prefabNameを代入する
            _attackCollider = _attackColliderInstantiate.InstantiatePrefab(parent);
            _idleState = idleState;
            _context = context;
            _rb = rb;

            _cts = new CancellationTokenSource();
        }
        
        public void StartAction()
        {
            if (_isCompleted)
            {
                Reset();
                _context.ChangeState(_idleState);
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