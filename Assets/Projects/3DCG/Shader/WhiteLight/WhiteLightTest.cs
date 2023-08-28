using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WhiteLightTest : MonoBehaviour
{
    [SerializeField] GameObject block;
    [SerializeField] Slider whiteRatioSlider;
    Material _material;
    
    void Start()
    {
        _material = block.GetComponent<Renderer>().material;
    }
    
    void Update()
    {
        // ShaderのNameとReferenceが異なることに注意
        _material.SetFloat("_WhiteRatio", whiteRatioSlider.value);
    }
}
