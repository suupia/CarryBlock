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
        public float ActionCoolTime => 0;
        ComponentPrefabInstantiate<AttackCollider> _attackColliderInstantiate;
        readonly AttackCollider _attackCollider;
        readonly string _prefabName = "TackleAttackCollider";

        public TackleAction(Transform parent)
        {
            _attackColliderInstantiate = new(
                new PrefabLoaderFromResources<AttackCollider>("Prefabs/Attacks"), 
                "AttackSphere"); // ToDo: _prefabNameを代入する
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
        readonly float _chargeTime;
        readonly JumpState _jumpState;
        readonly IBoss1Context _context;
        readonly CancellationTokenSource _cts;

        public ChargeJumpAction(float chargeTime, JumpState jumpState, IBoss1Context context)
        {
            _chargeTime = chargeTime;
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
                await UniTask.Delay(TimeSpan.FromSeconds(_chargeTime), cancellationToken: _cts.Token);
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
        public float ActionCoolTime { get; }
        ComponentPrefabInstantiate<AttackCollider> _attackColliderInstantiate;
        bool _isJumping;
        bool _isCompleted;
        public float jumpTime { get; init; }
        readonly AttackCollider _attackCollider;
        readonly string _prefabName = "JumpAttackCollider";
        readonly SearchPlayerState _searchPlayerState;
        readonly IBoss1Context _context;
        readonly Rigidbody _rb;
        readonly CancellationTokenSource _cts;

        public JumpAction(float actionCoolTime,SearchPlayerState searchPlayerState,IBoss1Context context, Transform parent, Rigidbody rb)
        {
            ActionCoolTime = actionCoolTime;
            _attackColliderInstantiate = new(
                new PrefabLoaderFromResources<AttackCollider>("Prefabs/Attacks"), 
                "AttackSphere"); // ToDo: _prefabNameを代入する
            _attackCollider = _attackColliderInstantiate.InstantiatePrefab(parent);
            _searchPlayerState = searchPlayerState;
            _context = context;
            _rb = rb;

            _cts = new CancellationTokenSource();
        }
        
        public void StartAction()
        {
            if (_isCompleted)
            {
                Reset();
                _context.ChangeState(_searchPlayerState);
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
        public float ActionCoolTime { get; }
        readonly Transform _transform;
        readonly string _prefabName = "SpitOutAttackCollider";

        public SpitOutAction(NetworkRunner runner, Transform transform,float actionCoolTime)
        {
            _transform = transform;
            ActionCoolTime = actionCoolTime;
            _attackColliderSpawner = new(runner,
                new PrefabLoaderFromResources<NetworkTargetAttackCollider>("Prefabs/Attacks"), 
                "AttackSphere"); // ToDo: _prefabNameを代入する
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

}