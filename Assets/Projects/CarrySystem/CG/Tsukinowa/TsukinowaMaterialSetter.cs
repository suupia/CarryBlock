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
        }
        
        async UniTaskVoid BlinkingTask()
        {
            Debug.Log($"BlinkingTask");
            float elapsedBlinkingTime = 0f;
            bool transparencyOn = false;

            while (elapsedBlinkingTime < _blinkingTime)
            {
                transparencyOn = !transparencyOn;
                float targetTransparency = transparencyOn ? 0f : 1f;

                skinnedMeshRenderer.material.SetFloat("_TransparencyRatio", targetTransparency);

                await UniTask.Delay((int)(_blinkInterval * 1000)); // Convert to milliseconds

                elapsedBlinkingTime += _blinkInterval;
            }

            // Optional: Reset transparency at the end
            skinnedMeshRenderer.material.SetFloat("_TransparencyRatio", 1f);
        }
    }

}
