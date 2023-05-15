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
        readonly string _prefabName = "TackleAttackCollider";

        public TackleAction(Transform parent)
        {
            _attackColliderInstantiate = new(
                new PrefabLoaderFromResources<AttackCollider>("Prefabs/Attacks"), 
                "AttackSphere"); // ToDo: _prefabNameを代入する
            _attackCollider = _attackColliderInstantiate.InstantiatePrefab(parent);
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