using Nuts.Utility.Scripts;
using Nuts.BattleSystem.Move.Scripts;
using Nuts.BattleSystem.Player.Scripts;
using UnityEngine;

namespace Nuts.BattleSystem.Enemy.Scripts
{
    public interface IEnemy : IMove
    {
        void Action();
        bool InAction();
        float ActionCooldown();
    }

    public interface IEnemyStats
    {
        void OnAttacked(ref NetworkPlayerStruct networkPlayerStruct, int damage);
    }

    public class EnemyInfo
    {
    }
}