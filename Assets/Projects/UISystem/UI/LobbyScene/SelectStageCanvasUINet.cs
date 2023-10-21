using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Map.Scripts;
using Fusion;
using Carry.GameSystem.LobbyScene.Scripts;
using Carry.NetworkUtility.Inputs.Scripts;
using Carry.UISystem.UI;
using Carry.Utility;
using Carry.Utility.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

#nullable enable

namespace Carry.UISystem.UI.LobbyScene
{
    public class SelectStageCanvasUINet : NetworkBehaviour
    {
        [SerializeField] GameObject viewObject = null!;
        [SerializeField] Transform buttonParent = null!;
        [SerializeField] CustomButton buttonPrefab;

        [Networked] protected NetworkButtons PreButtons { get; set; }

        MapKeyDataSelectorNet  _mapKeyDataSelectorNet = null!;
        StageIndexTransporter _stageIndexTransporter =  null!;
        InputAction _toggleSelectStageCanvas = null!;    
        
        [Inject]
        public void Construct(
            MapKeyDataSelectorNet mapKeyDataSelectorNet,
            StageIndexTransporter stageIndexTransporter
            )
        {
            _mapKeyDataSelectorNet = mapKeyDataSelectorNet;
            _stageIndexTransporter = stageIndexTransporter;
        }

        public override void Spawned()
        {
            viewObject.SetActive(false);

            if (!HasStateAuthority)return;

            var buttonCount = _mapKeyDataSelectorNet.MapKeyDataNetListCount;
            var stageButtons = buttonParent.GetComponentsInChildren<CustomButton>().ToList();
            for (int i = 0; i < buttonCount; i++)
            {
                var button = Instantiate(buttonPrefab, buttonParent);
                button.Init();
                stageButtons.Add(button);
            }
            
            var lobbyInitializer = FindObjectOfType<LobbyInitializer>();
            for(int i = 0; i< stageButtons.Count; i++)
            {
                var stageButton = stageButtons[i];
                stageButton.SetText($"Stage {i + 1}");
                var index = i;
                stageButton.AddListener(() =>
                {
                    _stageIndexTransporter.SetStageIndex(index);
                    lobbyInitializer.TransitionToGameScene();
                });
            }
            
            SetupToggleSelectStageCanvas();
        }
        
        
        // 以下の処理はInputAction系を初期化するところに移動させた方がよいかもしれない
        void SetupToggleSelectStageCanvas()
        {
            var inputActionMap = InputActionMapLoader.GetInputActionMap(InputActionMapLoader.ActionMapName.Default);
            _toggleSelectStageCanvas = inputActionMap.FindAction("ToggleSelectStageCanvas");
            _toggleSelectStageCanvas.performed += OnToggleSelectStageCanvas;
        }
        
        void OnToggleSelectStageCanvas(InputAction.CallbackContext context)
        {
            if(!HasStateAuthority)return;
            if (context.performed)
            {
                viewObject.SetActive(!viewObject.activeSelf);
            }
        }
    }
}

