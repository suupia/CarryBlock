using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneInitializer
{
    NetworkRunnerManager _runnerManager;
     NetworkPlayerContainer networkPlayerContainer = new();
     NetworkEnemyContainer networkEnemyContainer = new();


    public  SceneInitializer(SimulationBehaviour initializer)
    {
        _runnerManager = Object.FindObjectOfType<NetworkRunnerManager>(); 
        _runnerManager.Runner.AddSimulationBehaviour(initializer); // Register this class with the runner
        _runnerManager.Runner.AddCallbacks(new LocalInputPoller());

        
    }

    public async UniTask StartScene(string sessionName = default)
    {
        await _runnerManager.AttemptStartScene(sessionName);
    }
    

    // public void PlayerJoined(PlayerRef player)
    // {
    //     if (Runner.IsServer)
    //     {
    //         // var _= AsyncSpawnPlayer(player, token);
    //         playerSpawner.SpawnPlayer(player,networkPlayerContainer);
    //
    //         // Todo: RunnerがSetActiveシーンでシーンの切り替えをする時に対応するシーンマネジャーのUniTaskのキャンセルトークンを呼びたい
    //     }
    // }
    //
    //
    // public void PlayerLeft(PlayerRef player)
    // {
    //     if (Runner.IsServer)
    //     {
    //         playerSpawner.DespawnPlayer(player,networkPlayerContainer);
    //     }
    // }
    
}

