using Projects.BattleSystem.Enemy.Monster.Interfaces;
using UnityEngine;

namespace Projects.BattleSystem.Boss.Scripts
{
    
    public class TsukamotoAction :IEnemyAction
    {
        public bool IsActionCompleted { get; private set; }
        public float ActionCoolTime => 5.0f;

        public void StartAction()
        {
            Debug.Log("I start running");
            IsActionCompleted = false;
        }
        public void EndAction()
        {
            Debug.Log("I stop running");
            IsActionCompleted = true;
        }
    }
}