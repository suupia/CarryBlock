using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.VFX;

public class DashEffectSetter : MonoBehaviour
{
    [SerializeField] VisualEffect _dashEffect;
    void Start()
    {
        _dashEffect.SendEvent("Stop");
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _dashEffect.SendEvent("OnPlay");
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            _dashEffect.SendEvent("Stop");
        }
    }
}
