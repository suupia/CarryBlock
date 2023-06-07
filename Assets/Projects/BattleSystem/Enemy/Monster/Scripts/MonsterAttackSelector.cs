using Nuts.BattleSystem.Enemy.Monster.Interfaces;
using UnityEngine;

namespace Nuts.BattleSystem.Boss.Scripts
{
    public class RandomActionSelector : IBoss1ActionSelector
    {
        public IMonster1State SelectAction(params IMonster1State[] attacks)
        {
            // 0からattacks.Length-1までのランダムな整数を取得
            var randomIndex = Random.Range(0, attacks.Length);
            return attacks[randomIndex];
        }
    }
    
    /// <summary>
    /// 特定のアクションだけを返す
    /// </summary>
    public class FixedActionSelector : IBoss1ActionSelector
    {
        readonly int _attackIndex;

        public FixedActionSelector(int attackIndex)
        {
            _attackIndex = attackIndex;
        }

        public IMonster1State SelectAction(params IMonster1State[] attacks)
        {
            if (0 <= _attackIndex && _attackIndex < attacks.Length) return attacks[_attackIndex];

            Debug.LogError($"_attackIndex({_attackIndex}) is out of range.");
            return attacks[0];
        }
    }
}