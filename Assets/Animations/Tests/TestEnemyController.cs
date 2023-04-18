using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Fusion;
using Main;
using UnityEngine;

namespace Animations.Tests
{
    [Serializable]
    public enum AttackAnimationKind
    {
        Trigger, Loop
    }
    public class TestEnemyController: NetworkBehaviour
    {
        [SerializeField] AttackAnimationKind attackAnimationKind;

        IAnimatorSimpleEnemyUnit _animatorSetter;
        
        
        GameObject _targetPlayerObj;

        public Action onDespawn = () => { };

        public override void Spawned()
        {
            //AnimatorSetterの初期化
            var animator = GetComponentInChildren<Animator>();
            _animatorSetter = attackAnimationKind switch
            {
                AttackAnimationKind.Trigger => new TriggerEnemyAnimatorSetter(new TriggerEnemyAnimatorSetterInfo()
                {
                    Animator = animator
                }),
                AttackAnimationKind.Loop => new LoopEnemyAnimatorSetter(new LoopEnemyAnimatorSetterInfo()
                {
                    Animator = animator
                }),
                _ => throw new ArgumentOutOfRangeException()
            };
            //until here
            
            _animatorSetter.OnSpawn();
        }
        
        void Search()
        {
            //Stub
        }

        void Chase()
        {
            var direction = Utility.SetYToZero(_targetPlayerObj.transform.position - gameObject.transform.position).normalized;
            //渡す値については後で検証。現状はdirection.magnitudeの値のみを使用中
            _animatorSetter.OnMove(direction);
        }


        public async Task OnDefeated()
        {
            // onDespawn();
            //onDespawnを今後どのように扱うかわからないので、個別に呼ぶ
            //自由に変えてもらって構わない
            _animatorSetter.OnDead();

            //アニメーションの分待つ必要がある
            await UniTask.Delay(TimeSpan.FromSeconds(3));
            Runner.Despawn(Object);
        }
    }
}