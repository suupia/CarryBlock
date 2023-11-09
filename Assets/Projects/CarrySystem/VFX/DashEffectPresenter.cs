using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

namespace Carry.CarrySystem.Player.Scripts
{
    [RequireComponent(typeof(VisualEffect))]
    public class DashEffectPresenter : NetworkBehaviour
    {
        public  struct DashEffectData : INetworkStruct
        {
            public bool IsDashing;
        }
        [Networked] ref DashEffectData Data => ref MakeRef<DashEffectData>();
        
        bool _isDashingLocal;
        VisualEffect _dashEffect;
        public void Init(IDashExecutor dashExecutor)
        {
            dashExecutor.SetDashEffectPresenter(this);
            
        }

        void Awake()
        {
            _dashEffect = GetComponent<VisualEffect>();
            _dashEffect.SendEvent("Stop");
        }
        

        public override void Render()
        {
            if (_isDashingLocal != Data.IsDashing)
            {
                if (Data.IsDashing)
                {
                    // Debug.Log($"SendEvent Start Dash");
                    _dashEffect.SendEvent("Start");
                    _isDashingLocal = Data.IsDashing;
                }
                else
                {
                    // Debug.Log($"SendEvent Stop Dash");
                    _dashEffect.SendEvent("Stop");
                    _isDashingLocal = Data.IsDashing;
                }
            }
        }
    
        public void StartDash()
        {
            Data.IsDashing = true;
        }
        
        public void StopDash()
        {
            Data.IsDashing = false;
        }
    }
}
