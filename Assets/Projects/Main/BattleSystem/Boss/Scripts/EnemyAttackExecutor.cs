using System.Collections;
using System.Collections.Generic;
using Fusion;
using Main;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;

namespace Boss
{
    /// <summary>
    /// 何もしないIEnemyAttackExecutorの実装クラス
    /// </summary>
    public class DoNothingAttack : IEnemyAttackExecutor
    {
        public float AttackCoolTime => 0;

        public DoNothingAttack()
        {
        }

        public IUnitOnTargeted DetermineTarget(IEnumerable<IUnitOnTargeted> targetUnits)
        {
            return targetUnits.First();
        }

        public void Attack(IEnumerable<IUnitOnAttacked> targetUnits)
        {
            
        }
    }
    
    
}