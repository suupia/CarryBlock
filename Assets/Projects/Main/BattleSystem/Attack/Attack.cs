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
    /// Attackが呼ばれてから遅延してattackの攻撃を呼び出す
    /// 使用しなくても良いが、クライアントコードが複雑にならないことが期待できる。
    /// ここでのAttackはasyncになっている。キャストすればawait句が使用できる
    /// </summary>
    public class DelayAttack : IAttack
    {
        private float _delay;
        private IAttack _attack;

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
    /// </summary>
    public class RangeAttack : IAttack
    {
        private static GameObject _attackSpherePrefab = Resources.Load<GameObject>("Prefabs/Attacks/AttackSphere");
        private static string _attackSphereIdentifier = "AttackSphere";

        private GameObject _attackSphere;

        public RangeAttack([NotNull] GameObject gameObject, float radius = 0)
        {
            //プレハブの存在確認
            Assert.IsNotNull(_attackSpherePrefab);

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

    /// <summary>
    /// targetBufferを共有して、一番transformに近いものをtargetとする
    /// 使用される攻撃はtargetAttackで指定する
    /// </summary>
    public class ToNearestAttack : ITargetAttack
    {
        private ITargetAttack _targetAttack;
        private ISet<Transform> _targetBuffer;
        private readonly Transform _transform;


        public Transform Target
        {
            get => _targetAttack.Target;
            set => _targetAttack.Target = value;
        }

        public ToNearestAttack(Transform transform, ISet<Transform> targetBuffer, ITargetAttack targetAttack)
        {
            _transform = transform;
            _targetAttack = targetAttack;
            _targetBuffer = targetBuffer;
        }

        public void Attack()
        {
            if (_targetBuffer.Count == 0) return;

            Transform minTransform = null;
            float minDistance = float.PositiveInfinity;
            foreach (var transform in _targetBuffer)
            {
                var distance = Vector3.Distance(transform.position, _transform.position);
                if (distance < minDistance)
                {
                    minTransform = transform;
                    minDistance = distance;
                }
            }

            _targetAttack.Target = minTransform;
            _targetAttack.Attack();
        }
    }

    public class ToTargetAttack : ITargetAttack
    {
        private GameObject _gameObject;
        private IAttack _attack;

        public Transform Target { get; set; }

        public ToTargetAttack(GameObject gameObject, IAttack attack, Transform target = null)
        {
            Target = target;
            _attack = attack;
            _gameObject = gameObject;
        }

        public void Attack()
        {
            if (Target == null) return;
            _gameObject.transform.LookAt(Target);
            _attack.Attack();
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