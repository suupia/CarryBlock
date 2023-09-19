using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Projects.BattleSystem.LobbyScene.Scripts;
using Projects.NetworkUtility.Inputs.Scripts;
using Projects.UISystem.UI;
using TMPro;
using UnityEngine;
#nullable enable

namespace Carry.UISystem.UI.LobbyScene
{
    public class SelectStageCanvasUINet : NetworkBehaviour
    {
        [SerializeField] GameObject viewObject = null!;
        [SerializeField] Transform buttonParent = null!;
        List<CustomButton> stageButtons = new List<CustomButton>();
        
        [Networked] protected NetworkButtons PreButtons { get; set; }

        public override void Spawned()
        {
            viewObject.SetActive(false);

            if (!HasStateAuthority)return;

            stageButtons = buttonParent.GetComponentsInChildren<CustomButton>().ToList();
            
            var lobbyInitializer = FindObjectOfType<LobbyInitializer>();
            for(int i = 0; i< stageButtons.Count; i++)
            {
                var stageButton = stageButtons[i];
                stageButton.SetText($"Stage {i + 1}");
                stageButton.AddListener(() =>
                {
                    lobbyInitializer.TransitionToGameScene();
                });
            }
        }

        public override void FixedUpdateNetwork()
        {
            if(!HasStateAuthority)return;
            if (GetInput(out NetworkInputData input))
            {
                if (input.Buttons.WasPressed(PreButtons, PlayerOperation.ToggleSelectStageCanvas))
                {
                    Debug.Log($"Toggle SelectStageCanvas");
                    viewObject.SetActive(!viewObject.activeSelf);
                }


                PreButtons = input.Buttons;
            }
        }
        
    }

}

