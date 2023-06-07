# nullable  enable
using UnityEngine;

namespace Nuts.BattleSystem.Enemy.Monster.Interfaces
{
    public interface IEnemyAction
    {
        bool IsActionCompleted { get; }
        float ActionCoolTime { get; }　//ToDo: 関数でもいいかも
        void StartAction();
        void EndAction();
    }
    
    
    public interface IEnemyTargetAction : IEnemyAction
    {
        Transform? Target { get; set; }
    }
}