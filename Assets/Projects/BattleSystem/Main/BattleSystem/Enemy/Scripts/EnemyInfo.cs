using Main;
using Nuts.Projects.BattleSystem.Main.BattleSystem.Move;
using Nuts.Projects.BattleSystem.Main.BattleSystem.Player.Scripts;
using UnityEngine;

namespace Nuts.Projects.BattleSystem.Main.BattleSystem.Enemy.Scripts
{
    public interface IEnemy : IMove
    {
        void Move(Vector3 direction);
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