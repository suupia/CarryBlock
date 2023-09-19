using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Projects.BattleSystem.LobbyScene.Scripts;
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
        
        public override void Spawned()
        {
            Debug.Log($"SelectStageCanvasUINet.Spawned()");
            if (!HasStateAuthority)
            {
                viewObject.SetActive(false);
                return;
            } 
            
            stageButtons = buttonParent.GetComponentsInChildren<CustomButton>().ToList();
            Debug.Log($"CustomButtons : {stageButtons.Count}");
            
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
        
    }

}

