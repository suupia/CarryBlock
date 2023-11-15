using System;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Cysharp.Threading.Tasks.Triggers;
using Fusion;
using UnityEngine;
using VContainer;
using VContainer.Unity;
#nullable enable

namespace Carry.CarrySystem.FloorTimer.Scripts
{
    public class FloorProgressHandlerNet : NetworkBehaviour
    {
        // ToDo: デバッグ用のクラス

        EntityGridMapSwitcher _mapSwitcher = null!;
        public void Start()
        {
            // デバッグ用のクラスのため、コンテナには登録していない
            var resolver = FindObjectOfType<LifetimeScope>().Container;
            _mapSwitcher = resolver.Resolve<EntityGridMapSwitcher>();
        }
        
 # if UNITY_EDITOR
        void Update()
        {
            if(Runner == null) return;
            if(Runner.IsServer && Input.GetKeyDown(KeyCode.N))
            {
                _mapSwitcher.SwitchToNextMap();
            }

            if (Runner.IsServer && Input.GetKeyDown(KeyCode.B))
            {
                _mapSwitcher.SwitchToPreviousMap();
            }
            
        }
# endif
    }
}