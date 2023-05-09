using System;
using Cysharp.Threading.Tasks;
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
    public class RangeAttack : IEnemyAttack
    {
        private static readonly GameObject AttackSpherePrefab = Resources.Load<GameObject>("Prefabs/Attacks/AttackSphere");

        private const string AttackSphereIdentifier = "AttackSphere";
        
        public struct Context
        {
            public Transform Transform;
            public float Radius;
            public float AttackSphereLifeTime;
        }

        private Context _context;
        private readonly GameObject _attackSphere;

        /// <summary>
        /// radiusが0（指定しない）なら、プレハブの値が採用される
        /// </summary>
        /// <param name="context"></param>
        public RangeAttack(Context context)
        {
            //プレハブの存在確認
            Assert.IsNotNull(AttackSpherePrefab);
            //radiusは正
            Assert.IsTrue(context.Radius >= 0);
            Assert.IsTrue(context.AttackSphereLifeTime >= 0);

            var name = $"{AttackSphereIdentifier}:radius{context.Radius}";
            
            //攻撃用オブジェクトの初期化
            var transform = context.Transform.Find(name);
            _attackSphere = transform == null
                ? Object.Instantiate(AttackSpherePrefab, context.Transform)
                : transform.gameObject;

            _attackSphere.name = name;
            if (context.Radius != 0)
            {
                _attackSphere.GetComponent<SphereCollider>().radius = context.Radius;
            }
            
            if (context.AttackSphereLifeTime == 0)
            {
                context.AttackSphereLifeTime = 1f;
            }
            
            _attackSphere.SetActive(false);
            
            _context = context;

        }

        public async void Attack()
        {
            _attackSphere.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(_context.AttackSphereLifeTime));
            if (_attackSphere != null)
            {
                _attackSphere.SetActive(false);
            }
        }
    }
}