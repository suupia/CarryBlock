using System.Collections;
using System.Collections.Generic;
using Fusion;
using Main;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;

# nullable enable

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
    

    public class TackleAction : IEnemyActionExecutor
    {
        public float ActionCoolTime => 0;
        ComponentPrefabInstantiate<AttackCollider> _attackColliderInstantiate;
        readonly AttackCollider _attackCollider;

        public TackleAction(Transform parent,  float sphereRadius = 0)
        {
            _attackColliderInstantiate = new(
                new PrefabLoaderFromResources<AttackCollider>("Prefabs/Attacks"), 
                "AttackSphere");
            _attackCollider = _attackColliderInstantiate.InstantiatePrefab(parent);
            if(sphereRadius != 0 )_attackCollider.Radius = sphereRadius; // 引数が0の場合はプレハブの値を使用する
        }
        
        
        public void StartAction()
        {
            _attackCollider.gameObject.SetActive(true);
        }

        public void EndAction()
        {
            _attackCollider.gameObject.SetActive(false);
        }
    }

}