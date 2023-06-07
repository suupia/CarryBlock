using Main;
using Nuts.BattleSystem.Move.Scripts;
using Nuts.BattleSystem.Enemy.Scripts.Player.Scripts;
using UnityEngine;

namespace Nuts.BattleSystem.Enemy.Scripts
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