using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;
[RequireComponent(typeof(VisualEffect))]
public class DashEffectSetter : MonoBehaviour
{
    VisualEffect _dashEffect;
    void Start()
    {
        _dashEffect = GetComponent<VisualEffect>();
        _dashEffect.SendEvent("Stop");
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _dashEffect.SendEvent("Start");
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            _dashEffect.SendEvent("Stop");
        }
    }
}
