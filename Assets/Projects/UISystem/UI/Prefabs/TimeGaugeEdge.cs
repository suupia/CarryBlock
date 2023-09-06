using System;
using System.Collections;
using System.Collections.Generic;
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
            Debug.Log($"_width: {_width}");
        }

        void Update()
        {
            edgeTransform.localPosition = new Vector3(
                _width * fillImage.fillAmount - _width / 2,
                edgeTransform.localPosition.y,
                edgeTransform.localPosition.z
            );
            Debug.Log($"_width * fillImage.fillAmount : {_width * fillImage.fillAmount}, _width / 2: {_width / 2} x : {edgeTransform.position.x}");
        }
    }
}
