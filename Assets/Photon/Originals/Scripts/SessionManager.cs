using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;

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
    NetworkRunner runner;

    //Get roomName from UI component.
    public string RoomName { get; set; }

    // Start is called before the first frame update
    private void Start()
    {
        //Init NetworkRunner. Allow player's inputs.
        runner = gameObject.AddComponent<NetworkRunner>();
        runner.ProvideInput = true;
    }

    async void StartGame(GameMode mode, string roomName)
    {
        if (roomName.IsNullOrEmpty()) roomName = "TestRoom";

        //SceneManager �́A�V�[���ɒ��ڔz�u����� NetworkObjects �̃C���X�^���X������������
        await runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomName,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        runner.SetActiveScene(SceneName.LobbyScene);
    }

    //Will be called by UI component
    public void StartGameWithRoomName()
    {
        StartGame(GameMode.AutoHostOrClient, RoomName);
    }
}