using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
#nullable enable

namespace Carry.UISystem.UI.CarryScene
{
    public class TimeGaugeEdge : MonoBehaviour
    {
        [SerializeField] Image fillImage = null!;
        [SerializeField] RectTransform edgeTransform = null!;

        float _width;

        void Start()
        {
            _width = fillImage.rectTransform.rect.width;
        }

        void Update()
        {
            if(float.IsNaN(fillImage.fillAmount) ) return;
            
            edgeTransform.localPosition = new Vector3(
                _width * fillImage.fillAmount - _width / 2,
                edgeTransform.localPosition.y,
                edgeTransform.localPosition.z
            );
        }
    }
}
