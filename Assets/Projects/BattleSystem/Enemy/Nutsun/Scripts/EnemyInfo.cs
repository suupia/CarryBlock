using Projects.Utility.Scripts;
using Projects.BattleSystem.Move.Scripts;
using Projects.BattleSystem.Player.Scripts;
using UnityEngine;

namespace Projects.BattleSystem.Enemy.Scripts
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