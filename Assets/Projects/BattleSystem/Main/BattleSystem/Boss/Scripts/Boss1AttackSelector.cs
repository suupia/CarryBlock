using UnityEngine;

namespace Boss
{
    public interface IBoss1AttackSelector
    {
        IBoss1State SelectAction(params IBoss1State[] attacks);
    }

    public class RandomAttackSelector : IBoss1AttackSelector
    {
        public IBoss1State SelectAction(params IBoss1State[] attacks)
        {
            // 0からattacks.Length-1までのランダムな整数を取得
            var randomIndex = Random.Range(0, attacks.Length);
            return attacks[randomIndex];
        }
    }
}