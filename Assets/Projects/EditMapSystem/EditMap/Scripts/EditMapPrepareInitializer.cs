using System.Collections;
using System.Collections.Generic;
using Nuts.BattleSystem.Scripts;
using Nuts.NetworkUtility.NetworkRunnerManager.Scripts;
using UnityEngine;

public class EditMapPrepareInitializer : MonoBehaviour
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
        Debug.Log("Transitioning to EditMapScene");
        SceneTransition.TransitioningScene(runnerManager.Runner, SceneName.EditMapScene);
        _isStarted = true;
    }
}
