using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;
[RequireComponent(typeof(VisualEffect))]
public class DashEffectSetter : NetworkBehaviour
{
    public  struct DashEffectData : INetworkStruct
    {
        public bool IsDashing;
    }
    [Networked] ref DashEffectData Data => ref MakeRef<DashEffectData>();
    
    VisualEffect _dashEffect;
    void Start()
    {
        _dashEffect = GetComponent<VisualEffect>();
        _dashEffect.SendEvent("Stop");
    }
    
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.A))
    //     {
    //         _dashEffect.SendEvent("Start");
    //     }
    //
    //     if (Input.GetKeyUp(KeyCode.A))
    //     {
    //         _dashEffect.SendEvent("Stop");
    //     }
    // }

    public override void Render()
    {
        if (Data.IsDashing)
        {
            _dashEffect.SendEvent("Start");
        }
        else
        {
            _dashEffect.SendEvent("Stop");
        }
    }

    public void OnStart()
    {
        Data.IsDashing = true;
    }
    
    public void OnStop()
    {
        Data.IsDashing = false;
    }
}
