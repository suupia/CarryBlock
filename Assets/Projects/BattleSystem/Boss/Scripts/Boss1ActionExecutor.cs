# nullable enable

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Fusion;
using Nuts.BattleSystem.Enemy.Scripts;
using Nuts.BattleSystem.Move.Scripts;
using Nuts.BattleSystem.Enemy.Scripts.Spawners;
using UnityEngine;

namespace Nuts.BattleSystem.Boss.Scripts
{
    /// <summary>
    /// 何もしないIEnemyAttackExecutorの実装クラス
    /// </summary>
    public class DoNothingAction : IEnemyActionExecutor
    {
        public bool IsActionCompleted { get; set; } = false;

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
        public bool IsActionCompleted { get; set; }
        public float ActionCoolTime => 0.6f;
        bool _isDelaying;
        CancellationTokenSource _cts;
        public float delayTime { get; init; } = 0.3f;

        public IdleAction()
        {
            _cts = new CancellationTokenSource();
        }
        
        
        public void StartAction()
        {
            Debug.Log($"IdleAction StartAction _isCompleted:{IsActionCompleted}");
            Delay().Forget();
            
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
            IsActionCompleted = true;
        }

        void Reset()
        {
            _isDelaying = false;
            IsActionCompleted = false;
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        }
    }

    public class TackleAction : IEnemyActionExecutor
    {
        public bool IsActionCompleted { get; set; } = false;
        public float ActionCoolTime { get; init; } = 1.0f;
        ComponentPrefabInstantiate<AttackCollider> _attackColliderInstantiate;
        readonly AttackCollider _attackCollider;
        readonly string _prefabName = "TackleCollider";

        public TackleAction(Transform parent)
        {
            _attackColliderInstantiate = new(
                new PrefabLoaderFromResources<AttackCollider>("Prefabs/Attacks"), 
                _prefabName); // ToDo: _prefabNameを代入する
            _attackCollider = _attackColliderInstantiate.InstantiatePrefab(parent);
            _attackCollider.gameObject.SetActive(false);
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
        public  bool IsActionCompleted { get; set; }
        public float ActionCoolTime => 0.5f;
        bool _isCharging;
        public float chargeTime { get; init; } = 0.5f;
        CancellationTokenSource _cts;

        public ChargeJumpAction()
        {
            _cts = new CancellationTokenSource();
        }
        
        
        public void StartAction()
        {
            if (IsActionCompleted)
            {
                Reset();
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
            IsActionCompleted = true;
        }

        void Reset()
        {
            _isCharging = false;
            IsActionCompleted = false;
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        }
    }

    public class JumpAction : IEnemyActionExecutor
    {
       public  bool IsActionCompleted { get; set; }
        public float ActionCoolTime { get; init; } = 4.0f;
        ComponentPrefabInstantiate<AttackCollider> _attackColliderInstantiate;
        bool _isJumping;
        public float jumpTime { get; init; } = 2f;
        readonly AttackCollider _attackCollider;
        readonly string _prefabName = "JumpAttackCollider";
        readonly Rigidbody _rb;
         CancellationTokenSource _cts;

        public JumpAction(Transform parent, Rigidbody rb)
        {
            _attackColliderInstantiate = new(
                new PrefabLoaderFromResources<AttackCollider>("Prefabs/Attacks"), 
                "JumpCollider"); // ToDo: _prefabNameを代入する
            _attackCollider = _attackColliderInstantiate.InstantiatePrefab(parent);
            _attackCollider.gameObject.SetActive(false);
            _rb = rb;

            _cts = new CancellationTokenSource();
        }
        
        public void StartAction()
        {
            Debug.Log($"JumpAction StartAction _isCompleted:{IsActionCompleted}");
            if (IsActionCompleted)
            {
                Reset();
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
            Debug.Log($"Jump!!!!");
            MoveUtility.Jump(_rb,jumpTime);
            _attackCollider.gameObject.SetActive(true);

            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(jumpTime), cancellationToken: _cts.Token);
            }catch(OperationCanceledException)
            {
                Reset();
                return;
            }
            _isJumping = false;
            IsActionCompleted = true;
        }

        void Reset()
        {
            _attackCollider.gameObject.SetActive(false);
            _isJumping = false;
            IsActionCompleted = false;
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        }
    }

    public class SpitOutAction : IEnemyTargetActionExecutor
    {
        public bool IsActionCompleted { get; set; } = false;
        public float ActionCoolTime { get; init; } = 4.0f;
        public Transform Target { get; set; }
        NetworkBehaviourPrefabSpawner<NetworkTargetAttackCollider> _attackColliderSpawner;
        readonly float _yOffset = 1.0f;
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
            var pos = _transform.position + new Vector3(0, _yOffset, 0);
            var targetAttack = _attackColliderSpawner.SpawnPrefab(pos, Quaternion.identity, PlayerRef.None);
            targetAttack.Init(Target);
        }

        public void EndAction()
        {
            // 特になし
        }
    }

    public class VacuumAction : IEnemyTargetActionExecutor
    {
        public bool IsActionCompleted { get; set; } = false;
        public float ActionCoolTime { get; init; } = 4.0f;

        public Transform? Target { get; set; }
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