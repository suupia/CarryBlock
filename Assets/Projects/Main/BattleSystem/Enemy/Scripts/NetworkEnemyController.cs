using System;
using System.Linq;
using Fusion;
using UnityEngine;
using Main;

namespace Enemy
{
    [RequireComponent(typeof(NetworkRigidbody))]
    public class NetworkEnemyController : PoolableObject
    {
        public enum EnemyState
        {
            Idle,
            ChasingPlayer
        }

        [SerializeField] NetworkPrefabRef resourcePrefab;
        readonly float _detectionRange = 30;

        bool _isInitialized;

        IMove _move;

        Rigidbody _rb;

        EnemyState _state = EnemyState.Idle;
        GameObject _targetPlayerObj;

        public Action OnDespawn = () => { };

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

        void Search()
        {
            var colliders = Physics.OverlapSphere(gameObject.transform.position, _detectionRange);
            var players = colliders.Where(collider => collider.CompareTag("Player"))
                .Select(collider => collider.gameObject);
            Debug.Log($"players = {string.Join(",", players)}");
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