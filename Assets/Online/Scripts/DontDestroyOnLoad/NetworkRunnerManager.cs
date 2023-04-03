using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;

interface INetworkRunnerAccessor
{
    public NetworkRunner AccessRunner();
}

// TitleシーンならSessionNameを受け取ってからRunnerをインスタンス
// その他のシーンならStart()でRunnerをインスタンス
public class NetworkRunnerManager : MonoBehaviour
{
    [SerializeField] NetworkRunner runnerPrefab;

    //Get roomName from UI component.
    public string RoomName { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        //Init NetworkRunner. Allow player's inputs.
        
        // シーンを識別
        var activeScene = SceneManager.GetActiveScene();
        var sceneName = activeScene.name;
        if (sceneName == "TitleScene")
        {
            StartTitleScene();
        }
        else
        {
            StartOtherScene();
        }

    }

    void StartTitleScene()
    {
        // すぐにはRunnerをインスタンス化しない
        // セッション名が入力されてからインスタンス化
        // ToDo: TitleSceneの時の初期化処理を書く
    }

    async void StartOtherScene()
    {
        var runner = Instantiate(runnerPrefab);
        DontDestroyOnLoad(runner);
        
        await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.AutoHostOrClient,
            SessionName = "TestRoom",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    // async void StartGame(GameMode mode, string roomName)
    // {
    //    // if (roomName.IsNullOrEmpty()) roomName = "TestRoom";
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
    //     // runner.Spawn(phaseManager);
    //
    //     runner.SetActiveScene(SceneName.LobbyScene);
    // }
    //
    // //Will be called by UI component
    // public void StartGameWithRoomName()
    // {
    //     StartGame(GameMode.AutoHostOrClient, RoomName);
    // }
}
