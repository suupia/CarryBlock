using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Player.Scripts;
using UnityEngine;

namespace  Carry.CarrySystem.CG.Tsukinowa
{
    public class TsukinowaMaterialSetter : MonoBehaviour
    {
        [Tooltip("Red, Blue, Green, Yellow の順に設定する")]
        [SerializeField] Material[] clothMaterials;
        [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;

        public void SetClothMaterial(PlayerColorType colorType)
        {
            skinnedMeshRenderer.material = clothMaterials[(int) colorType];
        }
        
    }

}
