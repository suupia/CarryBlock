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
// シーン上にNetworkRunnerがないならインスタンス化し、runner.StartGame()を実行
public class NetworkRunnerManager : MonoBehaviour
{
    [SerializeField] GameObject fusionContainer;
    public NetworkRunner Runner => runner;

    [CanBeNull] NetworkRunner runner;

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
    
}
