using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

namespace Carry.CarrySystem.VFX.Scripts
{
    [RequireComponent(typeof(VisualEffect))]
    public class ReviveEffectPresenter : NetworkBehaviour
    {
        public struct ReviveEffectData : INetworkStruct
        {
            public bool IsRevived;
        }
        [Networked] ref ReviveEffectData Data => ref MakeRef<ReviveEffectData>();
        
        bool _isRevivedLocal;
        VisualEffect _reviveEffect;
        public void Init(IOnDamageExecutor onDamageExecutor)
        {
            onDamageExecutor.SetReviveEffectPresenter(this);
            
        }
        
        void Awake()
        {
            _reviveEffect = GetComponent<VisualEffect>();
            _reviveEffect.SendEvent("Stop");
        }
        
        public override void Render()
        {
            if (_isRevivedLocal != Data.IsRevived)
            {
                if (Data.IsRevived)
                {
                    //Debug.Log($"SendEvent Start Revive");
                    _reviveEffect.SendEvent("Start");
                    Data.IsRevived = false;
                }
                else
                {
                    // Debug.Log($"SendEvent Stop Revive");
                    _reviveEffect.SendEvent("Stop");
                    _isRevivedLocal = Data.IsRevived;
                }
            }
        }
        
        public void StartRevive()
        {
            Data.IsRevived = true;
        }
        public void StopRevive()
        {
            Data.IsRevived = false;
        }
    }
}