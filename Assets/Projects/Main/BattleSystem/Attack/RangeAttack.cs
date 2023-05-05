using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace Main
{
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
        private static readonly GameObject AttackSpherePrefab = Resources.Load<GameObject>("Prefabs/Attacks/AttackSphere");

        private const string AttackSphereIdentifier = "AttackSphere";

        private readonly GameObject _attackSphere;

        /// <summary>
        /// radiusが0（指定しない）なら、プレハブの値が採用される
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="radius"></param>
        public RangeAttack([NotNull] GameObject gameObject, float radius = 0)
        {
            //プレハブの存在確認
            Assert.IsNotNull(AttackSpherePrefab);
            //radiusは正
            Assert.IsTrue(radius >= 0);
            
            //攻撃用オブジェクトの初期化
            var transform = gameObject.transform.Find(AttackSphereIdentifier);
            _attackSphere = transform == null
                ? Object.Instantiate(AttackSpherePrefab, gameObject.transform)
                : transform.gameObject;

            _attackSphere.name = AttackSphereIdentifier;
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
}