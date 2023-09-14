using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.FloorTimer.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Fusion;
using UnityEngine;
using UnityEngine.UI;
using Projects.BattleSystem.Scripts;
using UnityEngine.Serialization;
using VContainer;
using UniRx;
#nullable enable

namespace Carry.UISystem.UI.CarryScene
{
    public class ResultCanvasNet : NetworkBehaviour
    {
        [SerializeField] GameObject viewGameObject = null!;
        [SerializeField] Button reStartButton = null!;
        [SerializeField] Button titleButton = null!;
        
        FloorTimerNet _floorTimerNet;

        [Networked] bool ViewActive { get; set; } = false;
        
        bool _viewActiveLocal = false;

        [Inject]
        public void Construct(
            FloorTimerNet floorTimerNet
        )
        {
            _floorTimerNet = floorTimerNet;

            this.ObserveEveryValueChanged(x => x._floorTimerNet.IsExpired)
                .Where(isExpired => isExpired)
                .Subscribe(_ =>
                {
                    viewGameObject.SetActive(true);
                    ViewActive = true;
                });
            
            Init();
        }

        void Start()
        {
               viewGameObject.SetActive(false);
        }
        
        void Init()
        {
            
            reStartButton.onClick.AddListener(() =>
            {
                Debug.Log("ReStartButton Clicked");
                SceneTransition.TransitioningScene(Runner, SceneName.CarryScene);
            });
            
            titleButton.onClick.AddListener(() =>
            {
                Debug.Log("QuitButton Clicked");
                SceneTransition.TransitioningScene(Runner, SceneName.TitleScene);

            });
        }
        
        public override void Render()
        {
            if (_viewActiveLocal != ViewActive)
            {
                _viewActiveLocal = ViewActive;
                viewGameObject.SetActive(ViewActive);
            }
        }
    }
}