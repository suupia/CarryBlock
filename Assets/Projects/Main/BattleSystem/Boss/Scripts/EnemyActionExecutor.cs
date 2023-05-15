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
    public class DoNothingAction : IEnemyActionExecutor
    {
        public float ActionCoolTime => 0;

        public DoNothingAction()
        {
        }

        public void StartAction() 
        {
        }
        
        public void EndAction()
        {
        }
    }
    
    // ToDo: TackleStateで使用されるAttackを作成する
    public class TackleAction : IEnemyActionExecutor
    {
        public float ActionCoolTime => 0;

        public TackleAction()
        {
        }
        
        public void StartAction()
        {
            
        }

        public void EndAction()
        {
            
        }
    }
    
}