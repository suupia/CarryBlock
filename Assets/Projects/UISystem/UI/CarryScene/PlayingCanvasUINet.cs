using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.FloorTimer.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Fusion;
using Projects.CarrySystem.Item.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;

#nullable enable

namespace Carry.UISystem.UI.CarryScene
{
    public class PlayingCanvasUINet : NetworkBehaviour
    {
        // NetworkObject must be attached to the parent of this script.
        [SerializeField] TextMeshProUGUI floorNumberText; // 現在のフロア数
        [SerializeField] TextMeshProUGUI floorTimerText;
        [FormerlySerializedAs("cointTotalText")] [SerializeField] TextMeshProUGUI coinTotalText;
        [SerializeField] Image floorTimerImage;
        [Networked] int FloorNumber { get; set; }
        [Networked] int MaxFloorNumber { get; set; }
        [Networked] float FloorRemainingSeconds { get; set; }
        [Networked] float FloorLimitSeconds { get; set; }
        [Networked] int CoinTotal { get; set; }

        IMapGetter _mapGetter = null!;
        FloorTimerNet _floorTimerNet = null!;
        MapKeyDataSelectorNet _mapKeyDataSelectorNet = null!;
        StageIndexTransporter _stageIndexTransporter = null!;
        TreasureCoinCounter _treasureCoinCounter = null!;

        int _maxFloorNumber;


        [Inject]
        public void Construct(
            IMapGetter mapGetter,
            FloorTimerNet floorTimerNet,
            MapKeyDataSelectorNet mapKeyDataSelectorNet,
            StageIndexTransporter stageIndexTransporter,
            TreasureCoinCounter treasureCoinCounter
        )
        {
            _mapGetter = mapGetter;
            _floorTimerNet = floorTimerNet;
            _mapKeyDataSelectorNet = mapKeyDataSelectorNet;
            _stageIndexTransporter = stageIndexTransporter;
            _treasureCoinCounter = treasureCoinCounter;

        }
        
        public override void Spawned()
        {
            if(!HasStateAuthority) return;
            var mapKeyDataList = _mapKeyDataSelectorNet.SelectMapKeyDataList(_stageIndexTransporter.StageIndex);
            _maxFloorNumber = mapKeyDataList.Count;
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;
            FloorNumber = _mapGetter.Index + 1;
            MaxFloorNumber =
                _maxFloorNumber; // to avoid : InvalidOperationException: Error when accessing PlayingCanvasUINet.MaxFloorNumber. Networked properties can only be accessed when Spawned() has been called.
            CoinTotal = _treasureCoinCounter.Count;
            FloorRemainingSeconds = _floorTimerNet.FloorRemainingSeconds;
            FloorLimitSeconds = _floorTimerNet.FloorLimitSeconds;
        }


        public override void Render()
        {
            Debug.Log($"PlayingCanvasUINet.Render()");
            floorNumberText.text = $"{FloorNumber} F / {MaxFloorNumber} F";
            floorTimerText.text = $"Time : {Mathf.Floor(FloorRemainingSeconds)}";
            // coinTotalText.text = $"Coin : {CoinTotal}";   // 文化祭では使わない

            floorTimerImage.fillAmount = FloorRemainingSeconds / FloorLimitSeconds;
        }
    }
}