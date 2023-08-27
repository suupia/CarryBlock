﻿using System;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Cysharp.Threading.Tasks.Triggers;
using Fusion;
using UnityEngine;
using VContainer;
using VContainer.Unity;


namespace Carry.CarrySystem.FloorTimer.Scripts
{
    public class FloorProgressHandlerNet : NetworkBehaviour
    {
        // ToDo: 仮置きクラス　ドメインの設計やコンテナの関係をしっかり考えれば、NetworkBehaviourである必要がないかも
        float _updateTime = 7;

        IMapUpdater _mapSwitcher;
        
        

# if UNITY_EDITOR
        void Update()
        {
            if(Runner == null) return;
            if(Runner.IsServer && Input.GetKeyDown(KeyCode.N))
            {
                _mapSwitcher.UpdateMap(MapKey.Default, 0);  // ToDo : 現時点では引数は使われていないので適当でよい
            }
            
        }
# endif
    }
}