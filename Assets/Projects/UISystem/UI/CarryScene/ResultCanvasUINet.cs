using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.FloorTimer.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.UI;
using Carry.GameSystem.Scripts;
using Carry.UISystem.UI;
using TMPro;
using UnityEngine.Serialization;
using VContainer;
using UniRx;
#nullable enable

namespace Carry.UISystem.UI.CarryScene
{
    public class ResultCanvasUINet : NetworkBehaviour
    {
        [SerializeField] TextMeshProUGUI resultText = null!;
        [SerializeField] TextMeshProUGUI clearedFloorText = null!;
        [SerializeField] TextMeshProUGUI clearTimeText = null!;
        [SerializeField] GameObject viewGameObject = null!;
        [SerializeField] CustomButton reStartButton = null!;
        [SerializeField] CustomButton titleButton = null!;
        
        [Networked] bool ViewActive { get; set; } = false;
        NetworkString<_16> ResultText { get; set; }
        NetworkString<_32> ClearedFloorText { get; set; } 
        NetworkString<_32> ClearTimeText { get; set; } 

        MapKeyDataSelectorNet _mapKeyDataSelectorNet;
        StageIndexTransporter _stageIndexTransporter;
        int _maxFloorNumber;
        int FloorNumber { get; set; }
        [Networked] int ClearedPercent { get; set; } 
        bool _viewActiveLocal = false;
        

        [Inject]
        public void Construct(
            FloorTimerNet floorTimerNet,
            IMapUpdater mapUpdater,
            MapKeyDataSelectorNet mapKeyDataSelectorNet,
            StageIndexTransporter stageIndexTransporter
        )
        {
            _mapKeyDataSelectorNet = mapKeyDataSelectorNet;
            _stageIndexTransporter = stageIndexTransporter;
            var mapKeyDataList = _mapKeyDataSelectorNet.SelectMapKeyDataList(_stageIndexTransporter.StageIndex);
            _maxFloorNumber = mapKeyDataList.Count;
            //ゲームオーバーの時のリザルト
            this.ObserveEveryValueChanged(_ => floorTimerNet.IsExpired)
                .Where(isExpired => isExpired)
                .Subscribe(_ =>
                {
                    viewGameObject.SetActive(true);
                    ViewActive = true;
                    ResultText = "GameOver";
                    FloorNumber = mapUpdater.Index + 1;
                    ClearedPercent = FloorNumber / _maxFloorNumber * 100;
                    ClearedFloorText = $"Cleared Floor Percent : {ClearedPercent} %";
                    ClearTimeText = "Time's up";
                });
            //ゲームクリアの時のリザルト
            this.ObserveEveryValueChanged(_ => floorTimerNet.IsCleared)
                .Where(isCleared => isCleared)
                .Subscribe(_ =>
                {
                    viewGameObject.SetActive(true);
                    ViewActive = true;
                    ResultText = "GameClear!!";
                    FloorNumber = mapUpdater.Index + 1;
                    ClearedPercent = FloorNumber / _maxFloorNumber * 100;
                    ClearedFloorText = $"Cleared Floor Percent : {ClearedPercent} %";
                    float clearTime = (int)(floorTimerNet.FloorLimitSeconds * _maxFloorNumber) -
                                    floorTimerNet.FloorRemainingSecondsSam;
                    int clearTimeMinutes = (int)clearTime / 60;
                    int clearTimeSeconds = (int)clearTime - clearTimeMinutes * 60;
                    if(clearTimeMinutes != 0)
                        ClearTimeText = $"Clear Time : {clearTimeMinutes}'{clearTimeSeconds}";
                    else
                        ClearTimeText = $"Clear Time : {clearTimeSeconds} s ";
                    //floorTimerNet.IsCleared = false;
                });
        }

        void Start()
        {
               viewGameObject.SetActive(false);
               
                           
               reStartButton.AddListener(() =>
               {
                   Debug.Log("ReStartButton Clicked");
                   SceneTransition.TransitioningScene(Runner, SceneName.CarryScene);
               });
            
               titleButton.AddListener(() =>
               {
                   Debug.Log("QuitButton Clicked");
                   SceneTransition.TransitioningScene(Runner, SceneName.TitleScene);

               });
        }
        
        public override void Spawned()
        {
            if (!HasStateAuthority)
            {
                // show button only for host player
                reStartButton.gameObject.SetActive(false);
                titleButton.gameObject.SetActive(false);
            }
        }
        
        public override void Render()
        {
            if (_viewActiveLocal != ViewActive)
            {
                _viewActiveLocal = ViewActive;
                viewGameObject.SetActive(ViewActive);
                resultText.text = ResultText.ToString();
                clearedFloorText.text = ClearedFloorText.ToString();
                clearTimeText.text = ClearTimeText.ToString();
                Debug.Log(ClearedFloorText);
            }
        }
    }
}