using System;
using Carry.CarrySystem.Map.Scripts;
using Cysharp.Threading.Tasks.Triggers;
using Fusion;
using UnityEngine;
using VContainer;
using VContainer.Unity;


namespace Carry.CarrySystem.CarryScene.Scripts
{
    public class FloorProgressHandler_Net : NetworkBehaviour
    {
        // ToDo: 仮置きクラス　ドメインの設計やコンテナの関係をしっかり考えれば、NetworkBehaviourである必要がないかも
        float _updateTime = 7;

        EntityGridMapSwitcher _mapSwitcher;
        public void Start()
        {
            var resolver = FindObjectOfType<LifetimeScope>().Container;
            _mapSwitcher = resolver.Resolve<EntityGridMapSwitcher>();
        }

        void Update()
        {
            if(Runner == null) return;
            if(Runner.IsServer && Input.GetKeyDown(KeyCode.N))
            {
                _mapSwitcher.NextFloor();
            }
            
        }

        // public override void FixedUpdateNetwork()
        // {
        //     // if (FloorTimer.ExpiredOrNotRunning(Runner))
        //     // {
        //     //     _mapSwitcher.NextFloor();
        //     //     FloorTimer = TickTimer.CreateFromSeconds(Runner, _updateTime);
        //     // }
        //     
        //     if(Runner.IsServer && Input.GetKeyDown(KeyCode.N))
        //     {
        //         _mapSwitcher.NextFloor();
        //     }
        // }
    }
}