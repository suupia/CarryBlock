using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TitleInitializer : SimulationBehaviour
{
    //Get roomName from UI component.
    public string RoomName { get; set; }

    
    //Called by UI component
    public async void StartGameWithRoomName()
    {
        var runnerManager = FindObjectOfType<NetworkRunnerManager>();
        await runnerManager.AttemptStartScene("RoomName");
        runnerManager.Runner.AddSimulationBehaviour(this); // Register this class with the runner
        Debug.Log($"Transitioning to LobbySceneTestRoom");
        SceneTransition.TransitioningScene(runnerManager.Runner,SceneName.LobbyScene);
    }
}