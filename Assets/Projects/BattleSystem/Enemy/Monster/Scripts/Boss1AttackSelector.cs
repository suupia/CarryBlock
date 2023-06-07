using UnityEngine;

namespace Nuts.BattleSystem.Boss.Scripts
{
    public interface IBoss1ActionSelector
    {
        IBoss1State SelectAction(params IBoss1State[] attacks);
    }

    public class RandomActionSelector : IBoss1ActionSelector
    {
        public IBoss1State SelectAction(params IBoss1State[] attacks)
        {
            // 0からattacks.Length-1までのランダムな整数を取得
            var randomIndex = Random.Range(0, attacks.Length);
            return attacks[randomIndex];
        }
    }
}