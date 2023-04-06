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
    [SerializeField] GameObject fusionContainer;
    public NetworkRunner Runner => runner;

    [CanBeNull] NetworkRunner runner;

    //Get roomName from UI component.
    public string RoomName { get; set; }

    
    public async UniTask StartScene(string sessionName = "TestRoom")
    {
        runner = FindObjectOfType<NetworkRunner>();
        if (runner == null)
        {
            var fusionContainerObj = Instantiate(fusionContainer);
            runner = fusionContainerObj.GetComponent<NetworkRunner>();
            DontDestroyOnLoad(runner);
        
            await runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.AutoHostOrClient,
                SessionName = sessionName,
                Scene = SceneManager.GetActiveScene().buildIndex,
                SceneManager = fusionContainerObj.AddComponent<NetworkSceneManagerDefault>()
            });
        }

    }

    // async UniTask StartTitleScene(string sessionName)
    // {
    //     runner = FindObjectOfType<NetworkRunner>();
    //       if (runner == null)
    //       {
    //           runner = Instantiate(fusionContainer);
    //           DontDestroyOnLoad(runner);
    //     
    //           await runner.StartGame(new StartGameArgs()
    //           {
    //               GameMode = GameMode.AutoHostOrClient,
    //               SessionName = sessionName,
    //               Scene = SceneManager.GetActiveScene().buildIndex,
    //               SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
    //           });
    //       }
    // }
    //
    // async UniTask StartOtherScene(string sessionName)
    // {
    //     runner = FindObjectOfType<NetworkRunner>();
    //     if (runner == null)
    //     {
    //         runner = Instantiate(fusionContainer);
    //         DontDestroyOnLoad(runner);
    //     
    //         await runner.StartGame(new StartGameArgs()
    //         {
    //             GameMode = GameMode.AutoHostOrClient,
    //             SessionName = sessionName,
    //             Scene = SceneManager.GetActiveScene().buildIndex,
    //             SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
    //         });
    //     }
    //
    // }

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
