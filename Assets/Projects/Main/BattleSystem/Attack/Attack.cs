using UnityEngine;

// 全体的な方針は以下の通り。これはIMoveも共通
//
//     - ネストを用いて擬似的に継承のような機能を実現する
//     - 細かい機能に分け、それらを組み合わせることで、攻撃を構築する
//     - シグネチャが複雑にならないように、必要な情報が２つ以上ある場合は、各クラスの内部構造体のContextでラップする
//     - FlutterやSwiftUI、Reactあたりの構文を参考にした。つまりUIの構築と同じような感覚で攻撃を構築できる
//
//     使用例)
// _attack = new ToNearestAttack(
//     new TargetBufferAttack.Context()
//     {
//         Transform = transform,
//         TargetBuffer = _targetBuffer
//     },
//     new ToTargetAttack(
//         gameObject,
//         new DelayAttack(
//             3,
//             new RangeAttack(gameObject, radius: 5)
//         )
//     )
// );
//
// ネストが多くなってしまうが、全体をネストの上位層から見ていくと、どのような攻撃かがわかるようになっている
//     まず、ToNearestAttackがあるので、TargetBufferにいる奴らから一番近い奴に攻撃が行われる
//     行われる攻撃は、第二引数のToTargetAttack
//     行われる攻撃は、第二引数のDelayAttack
//     行われる攻撃は、第二引数のRangeAttack
//
//     よって、この部分だけを見れば、「Attackを呼んだとき、一番近い奴に対して、３秒後に範囲５の攻撃が行われる」ことがわかるという設計
//     かつ、一番のメリットはIAttackを継承なし、かつクライアントコードの負担を最小に組み合わせることができるため、
// 様々な攻撃の構築が簡単にできる点である
namespace Main
{
    /// <summary>
    /// 攻撃を抽象化し、クライアントコードを簡潔に、再利用性を上げたい。
    /// void Attack()を実装するとする。
    /// 方針は実装者に委ねるが、主に３つあると考える
    ///
    /// 1. 攻撃用のColliderがついたGameObjectを動かすもの
    /// 2. Animationを使用するもの
    /// 3. 上記の攻撃をラップし、使いやすくしたもの
    ///
    /// 内部でどちらを採用するかは自由だが、必要なものがなにかをコメントで残すと理想的
    /// </summary>
    public interface IEnemyAttack
    {
        void Attack();
    }

    public interface ITargetAttack : IEnemyAttack
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
        protected IEnemyAttack _attack;

        // ReSharper disable once InconsistentNaming
        public IEnemyAttack attack => _attack;
        public override string ToString()
        {
            return $"({base.ToString()}+{_attack})";
        }
    }
    
    /// <summary>
    /// これから実装するAttackの仮置きとして使用する
    /// Wrapper系のAttackが機能しているかテストするときなど
    /// </summary>
    public class MockAttack : IEnemyAttack
    {
        public void Attack()
        {
            //Stub
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