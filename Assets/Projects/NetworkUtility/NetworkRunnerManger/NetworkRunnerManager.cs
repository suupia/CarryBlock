using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Nuts.NetworkUtility.Inputs.Scripts;
using UnityEngine.Serialization;
using  Nuts.NetworkUtility.ObjectPool.Scripts;

namespace Nuts.NetworkUtility.NetworkRunnerManager.Scripts
{
    // 全てのシーンにこれを配置しておけば、NetworkRunnerを使える
// シーン上にNetworkRunnerがないならインスタンス化し、runner.StartGame()を実行
public class NetworkRunnerManager : MonoBehaviour
{
    [SerializeField] NetworkRunner networkRunner;
    [SerializeField] NetworkSceneManagerDefault networkSceneManagerDefault;
    [SerializeField] NetworkObjectPoolDefault networkObjectPoolDefault;
    public NetworkRunner Runner => _runner;
    NetworkRunner _runner;
    public bool IsReady => Runner != null && Runner.SceneManager.IsReady(_runner);

    [SerializeField] private bool autoStart;
    [SerializeField] private string overrideRoomName;
    private async void Start()
    {
        if (autoStart) await AttemptStartScene(overrideRoomName);
    }

    public async UniTask AttemptStartScene(string sessionName = default)
    {
        sessionName ??= RandomString(5);
        _runner = FindObjectOfType<NetworkRunner>();
        if (_runner == null)
        {
            // Register this gameObject with DontDestroyOnLoad 
            DontDestroyOnLoad(gameObject);
            
            // Set up NetworkRunner
            _runner = Instantiate(networkRunner);
            DontDestroyOnLoad(_runner);
            _runner.AddCallbacks(new LocalInputPoller());
            
            // Set up SceneMangerDefault
            var sceneMangerDefault = Instantiate(networkSceneManagerDefault);
            DontDestroyOnLoad(sceneMangerDefault);
            
            await _runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.AutoHostOrClient,
                SessionName = sessionName,
                Scene = SceneManager.GetActiveScene().buildIndex,
                SceneManager = sceneMangerDefault,
                ObjectPool = networkObjectPoolDefault,
            });
            
            // Register to allow SceneLoadDone() of NetworkSceneManagerDefault to be called.
            _runner.AddSimulationBehaviour(networkObjectPoolDefault);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    // Create random char
    string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new System.Random();
        var result = new char[length];
        for (var i = 0; i < length; i++)
        {
            result[i] = chars[random.Next(chars.Length)];
        }

        return new string(result);
    }
}
}

