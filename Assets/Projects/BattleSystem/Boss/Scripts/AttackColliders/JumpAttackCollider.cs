using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Nuts.Projects.BattleSystem.Main.BattleSystem.Enemy.Scripts;

# nullable enable

namespace BattleSystem.Boss.AttackColliders
{
    public class JumpAttackCollider : AttackCollider
    {
        readonly int  _damage = 1;
        readonly float _attackInterval = 1;
        bool _isCoolDown;
        CancellationTokenSource? _cts;
        
        
        void OnEnable()
        {
            _cts = new CancellationTokenSource();
        }

        void OnDisable()
        {
            Reset();
        }


        void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            JumpAttack(other).Forget();
        }
        
        async UniTaskVoid JumpAttack(Collider other)
        {
            if (_isCoolDown) return;
            _isCoolDown = true;
            var player = other.GetComponent<IPlayerOnAttacked>();
            if (player == null)
            {
                Debug.LogError("The game object with the 'Player' tag does not have the 'IPlayerOnAttacked' component attached.");
                return;
            }
            try
            {
                Debug.Log($"JumpAttack ! damage = {_damage}");
                player.OnAttacked(_damage);
                await UniTask.Delay(TimeSpan.FromSeconds(_attackInterval), cancellationToken: _cts.Token);
                _isCoolDown = false;
            }catch(OperationCanceledException)
            {
                Reset();
            }
            
        }
        
        void Reset()
        {
            _isCoolDown = false;
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
        }
    }

}
