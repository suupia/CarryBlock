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
        private List<IDecorationEnemy> _decorations;
        private int _preAttackCount = 0;

        public DecorationEnemyContainer(params IDecorationEnemy[] decorations)
        {
            _decorations = decorations.ToList();
        }

        public void OnSpawn(NetworkDecorationEnemy networkStruct) { }
        
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
        private readonly List<IDecorationPlayer> _decorations;
        
        private int _preAttackCount = 0;
        private int _preMainActionCount = 0;
        private int _preHp = 0;

        public DecorationPlayerContainer(params IDecorationPlayer[] decorations)
        {
            _decorations = decorations.ToList();
        }

        public void OnSpawn()
        {
            _decorations.ForEach(d => d.OnSpawn());
        }

        public void OnMainAction(ref NetworkDecorationPlayer networkDecoration)
        {
            networkDecoration.MainActionCount++;
        }

        /// <summary>
        /// AttackとMainAttackとMoveとDamageとDeadの管理
        /// </summary>
        /// <param name="networkDecoration"></param>
        /// <param name="hp"></param>
        public void OnRender(ref NetworkDecorationPlayer networkDecoration, int hp)
        {
            _decorations.ForEach(d => d.OnMove());
            
            if (hp < _preHp)
            {
                _decorations.ForEach(d => d.OnDamage());
            }
            if (hp == 0)
            {
                _decorations.ForEach(d => d.OnDead());
            }
            _preHp = hp;

            if (networkDecoration.MainActionCount > _preMainActionCount)
            {
                _decorations.ForEach(d => d.OnMainAction());
                _preMainActionCount = networkDecoration.MainActionCount;
            }
            
            if (networkDecoration.AttackCount > _preAttackCount)
            {
                _decorations.ForEach(d => d.OnAttack());
                _preAttackCount = networkDecoration.AttackCount;
            }
        }

        public void OnAttack(ref NetworkDecorationPlayer networkDecoration)
        {
            networkDecoration.AttackCount++;
        }
    }
}