using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Player.Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace  Carry.CarrySystem.CG.Tsukinowa
{
    public class TsukinowaMaterialSetter : MonoBehaviour
    {
        [Tooltip("Red, Blue, Green, Yellow の順に設定する")]
        [SerializeField] Material[] clothMaterials;
        [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
        
        readonly float _blinkInterval = 0.1f;
        readonly float _blinkingTime = 1.0f;

        public void SetClothMaterial(PlayerColorType colorType)
        {
            skinnedMeshRenderer.material = clothMaterials[(int) colorType];
        }

        public void Blinking()
        {
            var _ = BlinkingTask();
            Debug.Log($"SkinnedMeshRenderer.materials.Length : {skinnedMeshRenderer.materials.Length}");

        }
        
        async UniTaskVoid BlinkingTask()
        {
            Debug.Log($"BlinkingTask");
            float elapsedBlinkingTime = 0f;
            bool isOn = false;

            while (elapsedBlinkingTime < _blinkingTime)
            {
                isOn = !isOn;

                foreach (var material in skinnedMeshRenderer.materials)
                {
                    if (isOn)
                    {
                        material.SetColor("_BaseColor", new Color(178/255.0f,34/255.0f,34/255.0f));
                    }
                    else
                    {
                        material.SetColor("_BaseColor", Color.white);
                    }
                }

                await UniTask.Delay((int)(_blinkInterval * 1000)); // Convert to milliseconds

                elapsedBlinkingTime += _blinkInterval;
            }

            // Optional: Reset transparency at the end
            skinnedMeshRenderer.material.SetFloat("_TransparencyRatio", 1f);
        }
    }

}
