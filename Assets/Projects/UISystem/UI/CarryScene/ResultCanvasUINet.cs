using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.FloorTimer.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Fusion;
using UnityEngine;
using UnityEngine.UI;
using Projects.BattleSystem.Scripts;
using Projects.UISystem.UI;
using TMPro;
using UnityEngine.Serialization;
using VContainer;
using UniRx;
#nullable enable

namespace Carry.UISystem.UI.CarryScene
{
    public class ResultCanvasUINet : NetworkBehaviour
    {
        [SerializeField] TextMeshProUGUI clearedFloorText = null!;
        [SerializeField] GameObject viewGameObject = null!;
        [SerializeField] CustomButton reStartButton = null!;
        [SerializeField] CustomButton titleButton = null!;
        
        [Networked] bool ViewActive { get; set; } = false;
        [Networked] int FloorNumber { get; set; }
        
        bool _viewActiveLocal = false;

        [Inject]
        public void Construct(
            FloorTimerNet floorTimerNet,
            IMapUpdater mapUpdater
        )
        {

            this.ObserveEveryValueChanged(_ => floorTimerNet.IsExpired)
                .Where(isExpired => isExpired)
                .Subscribe(_ =>
                {
                    viewGameObject.SetActive(true);
                    ViewActive = true;
                    FloorNumber = mapUpdater.Index + 1;
                });
            
            Init();
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
        
        void Init()
        {

        }
        
        public override void Render()
        {
            if (_viewActiveLocal != ViewActive)
            {
                _viewActiveLocal = ViewActive;
                viewGameObject.SetActive(ViewActive);
                clearedFloorText.text = $"Cleared Floor : {FloorNumber} F";
            }
        }
    }
}