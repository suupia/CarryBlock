using System;
using Cysharp.Threading.Tasks;
using Fusion;
using JetBrains.Annotations;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Main
{
    /// <summary>
    /// 打ち出す系の攻撃
    /// 打ち出すプレハブのパスを指定する
    /// このクラスではNetworkObjectを生成する想定、かつ
    /// ColliderとRigidBodyがある想定
    /// </summary>
    public class LaunchNetworkObjectAttack : ITargetAttack
    {
        /// <summary>
        /// PathかPrefabどちらかはnullでない必要がある
        /// Assertで管理される
        /// </summary>
        public struct Context
        {
            [NotNull] public NetworkRunner Runner;
            [NotNull] public Transform From;
            public Func<Vector3> GetOffset;
            public string Path;
            public GameObject Prefab;
            public float LaunchedObjectLifeTime;
            public float ThrowingTime;
        }

        public Transform Target { get; set; }

        private Context _context;
        private Rigidbody _rb;

        public LaunchNetworkObjectAttack(Context context, Transform target = null)
        {
            Assert.IsTrue(context.Prefab != null || context.Path != null);

            if (context.Prefab == null)
            {
                context.Prefab = Resources.Load<GameObject>(context.Path);
            }

            //プレハブの状態チェック
            Assert.IsNotNull(context.Prefab);
            Assert.IsNotNull(context.Prefab.GetComponent<NetworkObject>());
            Assert.IsNotNull(context.Prefab.GetComponent<NetworkRigidbody>());


            if (context.LaunchedObjectLifeTime <= 0)
            {
                context.LaunchedObjectLifeTime = 5f;
            }

            if (context.ThrowingTime <= 0)
            {
                context.ThrowingTime = 1f;
            }

            if (context.GetOffset == null)
            {
                context.GetOffset = () => Vector3.zero;
            }


            Target = target;
            _context = context;
        }

        public async void Attack()
        {
            if (Target == null) return;
            Debug.Log($"_context.Prefab={_context.Prefab}");
            Debug.Log($"_context.From={_context.From}");
            Debug.Log($"_context.GetOffset={_context.GetOffset}");
            var no = _context.Runner.Spawn(
                _context.Prefab,
                _context.From.position + _context.GetOffset(),
                _context.From.rotation,
                PlayerRef.None,
                (runner, obj) =>
                {
                    var rb = obj.GetComponent<Rigidbody>();
                    AttackUtility.Throw(
                        rb,
                        _context.From.position,
                        Target.position,
                        _context.ThrowingTime
                    );
                }
            );

            //TODO: DespawnはTickで管理したほうが良さそうなので、プレハブにMonoBehaviorをつけたほうがよいかも
            //もしそうするなら、動き方も委譲したほうが良い
            //Despawn
            await UniTask.Delay(TimeSpan.FromSeconds(_context.LaunchedObjectLifeTime));
            if (no != null && _context.Runner != null)
            {
                _context.Runner.Despawn(no);
            }
        }
    }
}