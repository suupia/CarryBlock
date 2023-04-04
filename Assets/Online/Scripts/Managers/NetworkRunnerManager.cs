using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Cysharp.Threading.Tasks;
using UnityEngine.Serialization;

// 全てのシーンにこれを配置しておけば、NetworkRunnerを使える
// TitleシーンならSessionNameを受け取ってからRunnerをインスタンス
// その他のシーンならStart()でRunnerをインスタンス
public class NetworkRunnerManager : MonoBehaviour
{
    [SerializeField] NetworkRunner fusionContainer;
    public NetworkRunner Runner => runner;

    [CanBeNull] NetworkRunner runner;

    //Get roomName from UI component.
    public string RoomName { get; set; }

    
    public async UniTask StartScene()
    {
        //Init NetworkRunner. Allow player's inputs.
        
        // シーンを識別
        var activeScene = SceneManager.GetActiveScene();
        var sceneName = activeScene.name;
        if (sceneName == "TitleScene")
        {
            await StartTitleScene();
        }
        else
        {
            await StartOtherScene();
        }

    }

    async UniTask StartTitleScene()
    {
        // すぐにはRunnerをインスタンス化しない
        // セッション名が入力されてからインスタンス化
        // ToDo: TitleSceneの時の初期化処理を書く
        // とりあえずは他のシーンと同じ

        runner = FindObjectOfType<NetworkRunner>();
          if (runner == null)
          {
              runner = Instantiate(fusionContainer);
              DontDestroyOnLoad(runner);
        
              await runner.StartGame(new StartGameArgs()
              {
                  GameMode = GameMode.AutoHostOrClient,
                  SessionName = "TestRoom",
                  Scene = SceneManager.GetActiveScene().buildIndex,
                  SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
              });
          }
    }

    async UniTask StartOtherScene()
    {
        runner = FindObjectOfType<NetworkRunner>();
        if (runner == null)
        {
            runner = Instantiate(fusionContainer);
            DontDestroyOnLoad(runner);
        
            await runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.AutoHostOrClient,
                SessionName = "TestRoom",
                Scene = SceneManager.GetActiveScene().buildIndex,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }

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
