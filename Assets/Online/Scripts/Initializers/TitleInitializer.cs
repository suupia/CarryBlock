using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TitleInitializer : SceneInitializer
{
    
    //Get roomName from UI component.
    public string RoomName { get; set; }

    
    //Called by UI component
    public async void StartGameWithRoomName()
    {
        await runnerManager.StartScene(RoomName);
        base.Init();
        Runner.SetActiveScene(SceneName.LobbyScene);

    }
}