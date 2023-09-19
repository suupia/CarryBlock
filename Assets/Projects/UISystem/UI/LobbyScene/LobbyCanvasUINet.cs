using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Projects.BattleSystem.LobbyScene.Scripts;
using Projects.UISystem.UI;
using UnityEngine;

#nullable enable

namespace Carry.UISystem.UI.LobbyScene
{
    public class LobbyCanvasUINet : NetworkBehaviour
    {
        [SerializeField] CustomButton startButton = null!;

        public override void Spawned()
        {
            if (!HasStateAuthority)
            {
                startButton.gameObject.SetActive(false);
            }
            
            var lobbyInitializer = FindObjectOfType<LobbyInitializer>();
            startButton.AddListener(() =>
            {
                lobbyInitializer.TransitionToGameScene();
            });
        }
    }
}