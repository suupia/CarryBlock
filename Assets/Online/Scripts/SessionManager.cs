using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This code is entry point of Fusion.
/// </summary>
/// 

public static class SceneName
{
    public static string GameScene = "GameScene";
    public static string LobbyScene = "LobbyScene";
}

public class SessionManager : MonoBehaviour
{
    [SerializeField] GameObject phaseManager;
    NetworkRunner _runner;

    //Get roomName from UI component.
    public string RoomName { get; set; }

    // Start is called before the first frame update
    private void Start()
    {
        //Init NetworkRunner. Allow player's inputs.
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;
    }

    async void StartGame(GameMode mode, string roomName)
    {
        if (string.IsNullOrEmpty(roomName)) roomName = "TestRoom";

        //SceneManager は、シーンに直接配置される NetworkObjects のインスタンス化を処理する
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomName,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        _runner.Spawn(phaseManager);

        _runner.SetActiveScene(SceneName.LobbyScene);
    }

    //Will be called by UI component
    public void StartGameWithRoomName()
    {
        StartGame(GameMode.AutoHostOrClient, RoomName);
    }
}
