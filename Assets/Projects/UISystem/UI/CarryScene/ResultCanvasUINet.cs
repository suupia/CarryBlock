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
        [Networked] NetworkBool IsClear { get; set; } = false;
        [Networked] int ClearedFloorNumber { get; set; } = 0;
        [Networked] int MaxFloorNumber { get; set; } = 0;
        [Networked] float ClearTime { get; set; } = 0;

        IMapGetter _mapGetter = null!;
        FloorTimerNet _floorTimerNet = null!;
        MapKeyDataSelectorNet _mapKeyDataSelectorNet = null!;
        StageIndexTransporter _stageIndexTransporter = null!;
        int _maxFloorNumber;
        bool _viewActiveLocal;
        

        [Inject]
        public void Construct(
            IMapGetter mapGetter,
            FloorTimerNet floorTimerNet,
            MapKeyDataSelectorNet mapKeyDataSelectorNet,
            StageIndexTransporter stageIndexTransporter
        )
        {
            _floorTimerNet = floorTimerNet;
            _mapGetter = mapGetter;
            _mapKeyDataSelectorNet = mapKeyDataSelectorNet;
            _stageIndexTransporter = stageIndexTransporter;

        }

        void Start()
        {
               viewGameObject.SetActive(false);

               reStartButton.AddListener(() =>
               {
                   Debug.Log("TransitioningScene to LobbyScene Clicked");
                   SceneTransition.TransitionSceneWithNetworkRunner(Runner, SceneName.LobbyScene);
               });
            
               titleButton.AddListener(() =>
               {
                   Debug.Log("TransitioningScene to TitleScene Clicked");
                   SceneTransition.TransitionSceneWithNetworkRunner(Runner, SceneName.TitleScene);

               });
        }

        public override void Spawned()
        {
            if (HasStateAuthority)
            {
                var mapKeyDataList = _mapKeyDataSelectorNet.SelectMapKeyDataList(_stageIndexTransporter.StageIndex);
                _maxFloorNumber = mapKeyDataList.Count;
                //ゲームオーバーの時のリザルト
                this.ObserveEveryValueChanged(_ => _floorTimerNet.IsExpired)
                    .Where(isExpired => isExpired)
                    .Subscribe(_ =>
                    {
                        ViewActive = true;
                        IsClear = false;
                        ClearedFloorNumber = _mapGetter.Index + 1;
                        MaxFloorNumber =  mapKeyDataList.Count;
                        ClearTime = _floorTimerNet.FloorLimitSeconds * _maxFloorNumber -
                                    _floorTimerNet.FloorRemainingSecondsSam;
                    });

                //ゲームクリアの時のリザルト
                this.ObserveEveryValueChanged(_ => _floorTimerNet.IsCleared)
                    .Where(isClear => isClear)
                    .Subscribe(_ =>
                    {
                        ViewActive = true;
                        IsClear = true;
                        ClearedFloorNumber = _mapGetter.Index + 1;
                        MaxFloorNumber =  mapKeyDataList.Count;
                        ClearTime = _floorTimerNet.FloorLimitSeconds * _maxFloorNumber -
                                    _floorTimerNet.FloorRemainingSecondsSam;
                    });

            }
            else
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
                resultText.text = IsClear ? $"GameClear!" : $"GameOver";
                clearedFloorText.text = $"Clear Rate : {(int)((float)ClearedFloorNumber/MaxFloorNumber * 100)} % ({ClearedFloorNumber} / {MaxFloorNumber})";
                clearTimeText.text = IsClear ? GetClearTimeText(ClearTime) : $"Time's up";
            }
        }
        
        
        String GetClearTimeText(float clearTime)
        {
            int clearTimeMinutes = (int)clearTime / 60;
            int clearTimeSeconds = (int)clearTime - clearTimeMinutes * 60;
            if(clearTimeMinutes != 0)
                return $"Clear Time : {clearTimeMinutes}'{clearTimeSeconds}";
            else
                return $"Clear Time : {clearTimeSeconds} s ";
        }
    }
}