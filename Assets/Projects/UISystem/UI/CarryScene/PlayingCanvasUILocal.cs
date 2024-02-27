using System;
using Carry.CarrySystem.FloorTimer.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Fusion;
using Projects.CarrySystem.Item.Scripts;
using Projects.MapMakerSystem.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;
using FloorTimerLocal = Carry.CarrySystem.FloorTimer.Scripts.FloorTimerLocal;

#nullable enable

namespace Carry.UISystem.UI.CarryScene
{
    public class PlayingCanvasUILocal: MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI floorNumberText;  // 現在のフロア数
        [SerializeField] TextMeshProUGUI floorTimerText;
        [SerializeField] Image floorTimerImage;

        FloorTimerLocal _floorTimerLocal = null!;


        [Inject]
        public void Construct(
            IMapGetter mapGetter,
            FloorTimerLocal floorTimerLocal
        )
        {
            _floorTimerLocal = floorTimerLocal;
            floorNumberText.text = $"{mapGetter.Index + 1} F";
        }

        void Update()
        {
            //Timerが有効な間だけUIを更新する
            if (!_floorTimerLocal.IsExpired) return;

            floorTimerText.text = $"Time : {Mathf.Floor(_floorTimerLocal.FloorRemainingSeconds)}";
            floorTimerImage.fillAmount = _floorTimerLocal.FloorRemainingTimeRatio;
        }
    }
}