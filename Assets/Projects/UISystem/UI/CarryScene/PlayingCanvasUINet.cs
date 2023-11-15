using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.FloorTimer.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Fusion;
using Projects.CarrySystem.Item.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

#nullable enable

namespace Carry.UISystem.UI.CarryScene
{
    public class PlayingCanvasUINet : NetworkBehaviour
    {
 // NetworkObject must be attached to the parent of this script.
        [SerializeField] TextMeshProUGUI floorNumberText;  // 現在のフロア数
        [SerializeField] TextMeshProUGUI floorTimerText;
        [SerializeField] TextMeshProUGUI cointTotalText;
        [SerializeField] Image floorTimerImage;
        [Networked] int FloorNumber { get; set; }
        [Networked] int CoinTotal { get; set; }
        
        IMapGetter _mapGetter = null!;
        FloorTimerNet _floorTimerNet = null!;
        TreasureCoinCounter _treasureCoinCounter = null!;


        [Inject]
        public void Construct(
            IMapGetter mapGetter,
            FloorTimerNet floorTimerNet,
            TreasureCoinCounter treasureCoinCounter
            )
        {
            _mapGetter = mapGetter;
            _floorTimerNet = floorTimerNet;
            _treasureCoinCounter = treasureCoinCounter;
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;
            FloorNumber = _mapGetter.Index + 1;
            CoinTotal = _treasureCoinCounter.Count;
        }


        public override void Render()
        {
            var remainingTime = _floorTimerNet.FloorRemainingSeconds;
            floorNumberText.text = $"{FloorNumber} F";
            floorTimerText.text = $"Time : {Mathf.Floor(remainingTime)}";
            cointTotalText.text = $"Coin : {CoinTotal}";

            floorTimerImage.fillAmount = remainingTime / _floorTimerNet.FloorLimitSeconds;
        }


    }
}