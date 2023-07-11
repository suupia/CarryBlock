using System;
using Nuts.BattleSystem.Scripts;
using Nuts.NetworkUtility.NetworkRunnerManager.Scripts;
using UnityEngine;

namespace Nuts.BattleSystem.TitleScene.Scripts
{
    public class TitleInitializer : MonoBehaviour
    {
        //Get roomName from UI component.
        public string RoomName { get; set; }
        
        bool _isStarted; // StarGameWithRoomName() is called only once.

        // Called by UI Button
        public async void StartGameWithRoomName()
        {
            if(_isStarted) return;
            var runnerManager = FindObjectOfType<NetworkRunnerManager>();
            await runnerManager.AttemptStartScene(RoomName);
            Debug.Log("Transitioning to LobbySceneTestRoom");
            SceneTransition.TransitioningScene(runnerManager.Runner, SceneName.LobbyScene);
            _isStarted = true;
        }
    }
}