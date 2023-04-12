using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] GameObject fusionContainer;
    public NetworkRunner Runner => _runner;

    [CanBeNull] NetworkRunner _runner;

    public async UniTask StartScene(string sessionName = default)
    {
        sessionName ??= RandomString(5);
        _runner = FindObjectOfType<NetworkRunner>();
        if (_runner == null)
        {
            var fusionContainerObj = Instantiate(fusionContainer);
            _runner = fusionContainerObj.GetComponent<NetworkRunner>();
            DontDestroyOnLoad(_runner);
        
            await _runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.AutoHostOrClient,
                SessionName = sessionName,
                Scene = SceneManager.GetActiveScene().buildIndex,
                SceneManager = fusionContainerObj.GetComponent<NetworkSceneManagerDefault>(),
                ObjectPool =  fusionContainerObj.GetComponent<NetworkObjectPoolDefault>(),
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
