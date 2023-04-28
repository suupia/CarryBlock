using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;


namespace Decoration
{
    public struct NetworkDecorationEnemy : INetworkStruct
    {
        public int AttackCount;
        public int PreHp;
    }

    public struct NetworkDecorationPlayer : INetworkStruct
    {
        public int MainActionCount;
        public int AttackCount;
    }


    public class DecorationEnemyContainer
    {
        private List<IEnemyDecoration> _decorations;
        private int _preAttackCount = 0;

        public DecorationEnemyContainer(params IEnemyDecoration[] decorations)
        {
            _decorations = decorations.ToList();
        }

        public void OnSpawn(NetworkDecorationEnemy networkStruct)
        {
        }
    }

    /// <summary>
    /// デコレーション系のすべてを管理する
    /// ローカル変数の管理をすべてこっちに持ってこれる
    /// 範囲が広い？個々で見れば小さいと思う
    /// アニメーション、サウンド、エフェクトなどのデコレーションを
    /// 任意に選択でき、かつ責任をすべてこのクラスにまとめる。
    /// これらはIDecoration系のインタフェースを継承していないといけない
    /// 
    /// 呼び出し側の制限は
    ///     正しくインタフェースの関数を呼ぶ
    ///     NetworkedプロパティとしてNetworkDecoration系の変数を持つ
    /// </summary>
    public class DecorationPlayerContainer
    {
        private readonly List<IPlayerDecoration> _decorations;

        private int _preAttackCount = 0;
        private int _preMainActionCount = 0;
        private int _preHp = 0;

        public DecorationPlayerContainer(params IPlayerDecoration[] decorations)
        {
            _decorations = decorations.ToList();
        }

        public void OnSpawned()
        {
            _decorations.ForEach(d => d.OnSpawned());
        }

        public void OnMainAction(ref NetworkDecorationPlayer networkDecoration)
        {
            networkDecoration.MainActionCount++;
        }


        public void OnAttacked(ref NetworkDecorationPlayer networkDecoration)
        {
            networkDecoration.AttackCount++;
        }

        /// <summary>
        /// AttackとMainAttackとMoveとDamageとDeadの管理
        /// </summary>
        /// <param name="networkDecoration"></param>
        /// <param name="hp"></param>
        public void OnRendered(ref NetworkDecorationPlayer networkDecoration, int hp)
        {
            _decorations.ForEach(d => d.OnMoved());

            if (hp != _preHp) OnHpChanged(hp);

            if (networkDecoration.MainActionCount > _preMainActionCount)
            {
                _decorations.ForEach(d => d.OnMainAction());
                _preMainActionCount = networkDecoration.MainActionCount;
            }

            if (networkDecoration.AttackCount > _preAttackCount)
            {
                _decorations.ForEach(d => d.OnAttacked());
                _preAttackCount = networkDecoration.AttackCount;
            }
        }

        private void OnHpChanged(int hp)
        {
            if (hp <= 0)
            {
                Debug.Log("OnDead!");
                _decorations.ForEach(d => d.OnDead());
            }
            else if (hp < _preHp)
            {
                Debug.Log("OnDamaged!");

                _decorations.ForEach(d => d.OnDamaged());
            }

            _preHp = hp;
        }
    }
}