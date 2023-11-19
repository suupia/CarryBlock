using Carry.CarrySystem.Player.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    [RequireComponent(typeof(VisualEffect))]
    public class DashEffectPresenterLocal : MonoBehaviour
    {
        // struct DashEffectData
        // {
        //     public bool IsDashing;
        // }
        //
        // DashEffectData _data;
        //
        // bool _isDashingLocal;
        // VisualEffect _dashEffect = null!;
        //
        // public void Init(IDashExecutor dashExecutor)
        // {
        //     dashExecutor.SetDashEffectPresenter(this);
        // }
        //
        // void Awake()
        // {
        //     _dashEffect = GetComponent<VisualEffect>();
        //     _dashEffect.SendEvent("Stop");
        // }
        //
        //
        // public void Update()
        // {
        //     if (_isDashingLocal != _data.IsDashing)
        //     {
        //         if (_data.IsDashing)
        //         {
        //             // Debug.Log($"SendEvent Start Dash");
        //             _dashEffect.SendEvent("Start");
        //             _isDashingLocal = _data.IsDashing;
        //         }
        //         else
        //         {
        //             // Debug.Log($"SendEvent Stop Dash");
        //             _dashEffect.SendEvent("Stop");
        //             _isDashingLocal = _data.IsDashing;
        //         }
        //     }
        // }
        //
        // public void StartDash()
        // {
        //     _data.IsDashing = true;
        // }
        //
        // public void StopDash()
        // {
        //     _data.IsDashing = false;
        // }
    }
}