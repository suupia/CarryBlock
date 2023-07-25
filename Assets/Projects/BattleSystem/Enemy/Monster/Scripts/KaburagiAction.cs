using System.Collections;
using System.Collections.Generic;
using Projects.BattleSystem.Enemy.Monster.Interfaces;
using UnityEngine;

namespace Projects.BattleSystem.Boss.Scripts
{
    public class KaburagiAction : IEnemyAction
    {
        public bool IsActionCompleted { get; set; } = false;

        public float ActionCoolTime => 0.5f;

        public void StartAction() 
        {
            IsActionCompleted = false;
        }
        
        public void EndAction()
        {
            IsActionCompleted = true;
        }
    }
}
