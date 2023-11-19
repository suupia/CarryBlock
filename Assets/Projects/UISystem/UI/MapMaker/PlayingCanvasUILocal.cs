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

namespace Carry.UISystem.UI.MapMaker
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
            Debug.Log($"floorNumberText: {floorNumberText}");
            Debug.Log($"_floorTimerLocal: {floorTimerLocal}");
        }

        void Update()
        {
            // Debug.Log($"floorNumberText: {floorNumberText}");
            // Debug.Log($"_floorTimerLocal: {_floorTimerLocal}");
            floorTimerText.text = $"Time : {Mathf.Floor(_floorTimerLocal.FloorRemainingSeconds)}";
            // coinTotalText.text = $"Coin : {_treasureCoinCounter.Count}";

            floorTimerImage.fillAmount = _floorTimerLocal.FloorRemainingTimeRatio;
        }
    }
}