using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TitleManger : NetworkSceneManager
{
    // async void Start()
    // {
    //     await base.Init();
    //
    //     await runnerManager.StartScene();
    //
    //     Debug.Log($"Runner:{Runner}, runnerManager.Runner:{runnerManager.Runner}");
    //     runnerManager.Runner.AddSimulationBehaviour(this); // Runnerに登録
    //     Debug.Log($"Runner:{Runner}");
    // }
    //
    // async void StartGame(GameMode mode, string roomName)
    // {
    //     if (string.IsNullOrEmpty(roomName)) roomName = "TestRoom";
    //
    //     //SceneManager は、シーンに直接配置される NetworkObjects のインスタンス化を処理する
    //     await runner.StartGame(new StartGameArgs()
    //     {
    //         GameMode = mode,
    //         SessionName = roomName,
    //         Scene = SceneManager.GetActiveScene().buildIndex,
    //         SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
    //     });
    //
    //     runner.Spawn(phaseManager);
    //
    //     runner.SetActiveScene(SceneName.LobbyScene);
    // }

    //Will be called by UI component
    public async void StartGameWithRoomName()
    {
        await base.Init();
        Runner.SetActiveScene(SceneName.LobbyScene);

    }
}