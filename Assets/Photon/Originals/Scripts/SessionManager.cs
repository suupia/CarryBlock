using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;

/// <summary>
/// This code is entry point of Fusion.
/// </summary>

public class SessionManager : MonoBehaviour
{
    public static readonly string lobbySceneName = "LobbyScene";

    NetworkRunner runner;
    NetworkSceneManagerBase sceneManager;

    //Get roomName from UI component.
    public string RoomName { get; set; }

    // Start is called before the first frame update
    private void Start()
    {
        //Init NetworkRunner. Allow player's inputs.
        runner = gameObject.AddComponent<NetworkRunner>();
        runner.ProvideInput = true;

        //Init SceneManager
        sceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>();
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
            SceneManager = sceneManager
        });

        runner.SetActiveScene(lobbySceneName);
    }

    //Will be called by UI component
    public void StartGameWithRoomName()
    {
        StartGame(GameMode.AutoHostOrClient, RoomName);
    }
}
