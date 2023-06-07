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
    [RequireComponent(typeof(Collider))]
    public class TackleAttackCollider : AttackCollider
    {
        readonly int  _damage = 1;
        readonly float _attackInterval = 1;
         bool _isCoolDown ;
         CancellationTokenSource? _cts;
            
        // 今のところ外部から引数をもらう必要がないのでInit()は不要
        
        // GameObjectのactiveを切り替えて制御する
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

            TackleAttack(other).Forget();

        }

        async UniTaskVoid TackleAttack(Collider other)
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
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
        }

    }
}
