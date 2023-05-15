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
        ComponentPrefabInstantiate<SphereCollider> _sphereColliderInstantiate;
        readonly SphereCollider _collider;

        public TackleAction(Transform parent,  float sphereRadius = 0)
        {
            _sphereColliderInstantiate = new(
                new PrefabLoaderFromResources<SphereCollider>("Prefabs/Attacks"), 
                "AttackSphere");
            _collider = _sphereColliderInstantiate.InstantiatePrefab(parent);
            if(sphereRadius != 0 )_collider.radius = sphereRadius; // 引数が0の場合はプレハブの値を使用する
        }
        
        
        public void StartAction()
        {
            _collider.enabled = true;
        }

        public void EndAction()
        {
            _collider.enabled = false;
        }
    }

}