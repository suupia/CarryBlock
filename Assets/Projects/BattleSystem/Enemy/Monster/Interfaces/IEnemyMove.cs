# nullable  enable
using UnityEngine;

namespace Nuts.BattleSystem.Enemy.Monster.Interfaces
{
    public interface IEnemyMove
    {
        void Move();
    }
    
    public interface IEnemyTargetMove : IEnemyMove
    {
        Transform Target { get; set; }
    }

}