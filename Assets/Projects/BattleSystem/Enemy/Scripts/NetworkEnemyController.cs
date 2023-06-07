using System;
using System.Linq;
using Fusion;
using Main;
using NetworkUtility.ObjectPool;
using Nuts.BattleSystem.Move.Scripts;
using Nuts.BattleSystem.Enemy.Scripts.Player.Attack;
using Nuts.BattleSystem.Player.Scripts;
using UnityEngine;

namespace Nuts.BattleSystem.Enemy.Scripts
{
    [RequireComponent(typeof(NetworkRigidbody))]
    public class NetworkEnemyController : PoolableObject , IEnemyOnAttacked
    {
        // ToDo: enemyObjectParentの子にモデルを配置する（まだ、Cubeが仮置きされている）
        // [SerializeField]  Transform enemyObjectParent; // The NetworkCharacterControllerPrototype interpolates this transform.
        public Transform InterpolationTransform => transform; // これは仮置きであることに注意。本来はenemyObjectParentを返す。
        [SerializeField] NetworkPrefabRef resourcePrefab;
        
        // Tmp
        [Networked] public int Hp { get; set; }
        
        public Action OnDespawn = () => { }; // EnemyContainerから削除する処理が入る
        
        readonly float _detectionRange = 30;
        

        Rigidbody _rb;
        GameObject _targetPlayerObj;
        IMove _move;
        EnemyState _state = EnemyState.Idle;

        bool _isInitialized;
        public enum EnemyState
        {
            Idle,
            ChasingPlayer
        }

        void OnDisable()
        {
            OnInactive();
        }

        void OnCollisionEnter(Collision other)
        {
            var player = other.gameObject.GetComponent<NetworkPlayerController>();
            if (player == null) return;
            player.OnAttacked(1);
        }

        public override void Spawned()
        {
            _rb = GetComponent<Rigidbody>();
            _move = new RegularMove
            {
                transform = transform,
                rd = _rb
            };
            Hp = 3;
            _isInitialized = true;
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;

            switch (_state)
            {
                case EnemyState.Idle:
                    Search();
                    break;
                case EnemyState.ChasingPlayer:
                    Chase();
                    break;
            }
        }

        public void OnAttacked(int damage)
        {
            if(!HasStateAuthority)return;
            // Debug.Log($"Hp = {Hp}");
            Hp -= damage;
            if(Hp <= 0)OnDefeated();
        }

        void Search()
        {
            var colliders = Physics.OverlapSphere(gameObject.transform.position, _detectionRange);
            var players = colliders.Where(collider => collider.CompareTag("Player"))
                .Select(collider => collider.gameObject);
            // Debug.Log($"players = {string.Join(",", players)}");
            if (players.Any())
            {
                _targetPlayerObj = Utility.ChooseRandomObject(players);
                _state = EnemyState.ChasingPlayer;
            }
            else
            {
                _state = EnemyState.Idle;
            }
        }

        void Chase()
        {
            var direction = Utility.SetYToZero(_targetPlayerObj.transform.position - gameObject.transform.position)
                .normalized;
            _move.Move(direction);
        }

        public void OnDefeated()
        {
            Debug.Log($"Runner = {Runner}"); // Todo: シーン切り替え時にRunnerがnullになっている時があるかも
            Runner.Spawn(resourcePrefab, transform.position, Quaternion.identity, PlayerRef.None);
            OnDespawn();
            Runner.Despawn(Object);
        }

        protected override void OnInactive()
        {
            if (!_isInitialized) return;
            _state = EnemyState.Idle;
        }
    }
}