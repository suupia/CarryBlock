using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Main
{
    /// <summary>
    /// 攻撃を抽象化し、クライアントコードを簡潔に、再利用性を上げたい。
    /// 方針は実装者に委ねるが、主に３つあると考える
    ///
    /// 1. 攻撃用のColliderがついたGameObjectを動かすもの
    /// 2. Animationを使用するもの
    /// 3. 上記の攻撃をラップし、使いやすくしたもの
    ///
    /// 内部でどちらを採用するかは自由だが、必要なものがなにかをコメントで残すと理想的
    /// </summary>
    public interface IAttack
    {
        void Attack();
    }
    
    public interface ITargetAttack : IAttack
    {
        Transform Target { get; set; }
    }

    /// <summary>
    /// いくつかのIAttack実装クラスの基底クラス
    /// _attackプロパティを持ち、
    /// ToStringをオーバーライドしていて、ネスト関係が見やすいようになっている
    /// </summary>
    public class AttackWrapper
    {
        // ReSharper disable once InconsistentNaming
        protected IAttack _attack;

        // ReSharper disable once InconsistentNaming
        public IAttack attack => _attack;
        public override string ToString()
        {
            return $"{base.ToString()}+{_attack}";
        }
    }


    /// <summary>
    /// Attackが呼ばれてから遅延してattackの攻撃を呼び出す
    /// 使用しなくても良いが、クライアントコードが複雑にならないことが期待できる。
    /// ここでのAttackはasyncになっている。キャストすればawait句が使用できる
    /// </summary>
    public class DelayAttack : AttackWrapper, IAttack
    {
        private float _delay;

        public DelayAttack(float delay, [NotNull] IAttack attack)
        {
            _delay = delay;
            _attack = attack;
        }

        public async void Attack()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_delay));
            _attack.Attack();
        }
    }

    /// <summary>
    /// 範囲攻撃をする。Radiusだけ渡せば、Attackを呼び出したタイミングで機能する
    /// 
    /// 実装は、攻撃用のプレハブをロードしてきて、インスタンス化
    /// このプレハブにはSphereColliderがついていて、あたったかどうかの判定を他に任せる
    ///
    /// 内部でInstantiateを使用しているが、コライダーのみのPrefabのため、見た目に影響がないかつ、
    /// Hostでのみ当たり判定を管理する予定なので問題ないはず
    /// 問題があれば、Runnerを受け取るようにする
    /// </summary>
    public class RangeAttack : IAttack
    {
        private static GameObject _attackSpherePrefab = Resources.Load<GameObject>("Prefabs/Attacks/AttackSphere");
        private static string _attackSphereIdentifier = "AttackSphere";

        private GameObject _attackSphere;

        /// <summary>
        /// radiusが0（指定しない）なら、プレハブの値が採用される
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="radius"></param>
        public RangeAttack([NotNull] GameObject gameObject, float radius = 0)
        {
            //プレハブの存在確認
            Assert.IsNotNull(_attackSpherePrefab);
            //radiusは正
            Assert.IsTrue(radius >= 0);
            
            //攻撃用オブジェクトの初期化
            var transform = gameObject.transform.Find(_attackSphereIdentifier);
            _attackSphere = transform == null
                ? Object.Instantiate(_attackSpherePrefab, gameObject.transform)
                : transform.gameObject;

            _attackSphere.name = _attackSphereIdentifier;
            if (radius != 0)
            {
                _attackSphere.GetComponent<SphereCollider>().radius = radius;
            }
            _attackSphere.SetActive(false);
        }

        public async void Attack()
        {
            _attackSphere.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            if (_attackSphere != null)
            {
                _attackSphere.SetActive(false);
            }
        }
    }





    public class MockAttack : IAttack
    {
        public void Attack()
        {
            //Impl Later...
        }
    }
    // public class ChooseRandomAttack : IAttack
    // {
    //     private List<IAttack> _attacks;
    //
    //     private IAttack RandomAttack =>
    //         _attacks[Random.Range(0, _attacks.Count)];
    //
    //     public ChooseRandomAttack(params IAttack[] attacks)
    //     {
    //         Assert.AreNotEqual(attacks.Length, 0);
    //
    //         _attacks = attacks.ToList();
    //     }
    //
    //     public void Attack()
    //     {
    //         RandomAttack.Attack();
    //     }
    // }
}