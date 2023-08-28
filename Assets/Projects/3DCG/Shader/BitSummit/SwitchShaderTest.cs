using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TimeControlShader
{
    public class SwitchShaderTest : MonoBehaviour
    {
        [SerializeField] Material testMaterial;
        [SerializeField] Slider whiteRatioSlider;
        [SerializeField] Slider effectRatioSlider;

        void Start()
        {
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                testMaterial.SetColor("_EffectColor", Color.red);
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                testMaterial.SetColor("_EffectColor", Color.green);
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                testMaterial.SetColor("_EffectColor", Color.blue);
            }

            testMaterial.SetFloat("_WhiteRatio", whiteRatioSlider.value);
            testMaterial.SetFloat("_EffectRatio", effectRatioSlider.value);
        }
    }
}