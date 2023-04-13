using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Cysharp.Threading.Tasks;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

// 全てのシーンにこれを配置しておけば、NetworkRunnerを使える
// シーン上にNetworkRunnerがないならインスタンス化し、runner.StartGame()を実行
public class NetworkRunnerManager : MonoBehaviour
{
    [SerializeField] NetworkRunner networkRunner;
    [SerializeField] NetworkSceneManagerDefault networkSceneManagerDefault;
    [SerializeField] NetworkObjectPoolDefault networkObjectPoolDefault;
    public NetworkRunner Runner => _runner;
    NetworkRunner _runner;

    public async UniTask AttemptStartScene(string sessionName = default)
    {
        sessionName ??= RandomString(5);
        _runner = FindObjectOfType<NetworkRunner>();
        if (_runner == null)
        {
            // Set up NetworkRunner
            _runner = Instantiate(networkRunner);
            DontDestroyOnLoad(_runner);
            _runner.AddCallbacks(new LocalInputPoller());
            
            // Set up NetworkSceneManager
            var networkSceneManager = Instantiate(networkSceneManagerDefault);
            DontDestroyOnLoad(networkSceneManager);

            await _runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.AutoHostOrClient,
                SessionName = sessionName,
                Scene = SceneManager.GetActiveScene().buildIndex,
                SceneManager = networkSceneManager,
                ObjectPool = networkObjectPoolDefault,
            });

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