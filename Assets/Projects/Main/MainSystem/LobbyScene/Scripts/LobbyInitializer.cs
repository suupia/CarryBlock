using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Main
{
    [DisallowMultipleComponent]
public class LobbyInitializer : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    NetworkPlayerContainer _networkPlayerContainer = new();
    NetworkEnemyContainer _networkEnemyContainer = new();
    NetworkPlayerSpawner _networkPlayerSpawner;
    EnemySpawner _enemySpawner;
    
    async void  Start()
    {
        var runnerManager = FindObjectOfType<NetworkRunnerManager>();
        // Runner.StartGame() if it has not been run.
        await runnerManager.AttemptStartScene("LobbySceneTestRoom");
        runnerManager.Runner.AddSimulationBehaviour(this); // Register this class with the runner
        await UniTask.WaitUntil(() => Runner.SceneManager.IsReady(Runner), cancellationToken: new CancellationToken());
        
        // Domain
        var playerSpawner = new NetworkBehaviourSpawner<NetworkPlayerController>(Runner,
            new PrefabLoaderFromResources<NetworkPlayerController>("Prefabs/Players"));
        _networkPlayerSpawner = new NetworkPlayerSpawner(Runner, playerSpawner);
        _enemySpawner = new EnemySpawner(Runner);
        
        if (Runner.IsServer)
        {
            _networkPlayerSpawner.RespawnAllPlayer(_networkPlayerContainer);
        }

        if (Runner.IsServer)
        {
            _networkEnemyContainer.MaxEnemyCount = 5;
            var _ = _enemySpawner.StartSimpleSpawner(0, 5f,_networkEnemyContainer);
        }
    }

    // ボタンから呼び出す
    public void TransitionToGameScene()
    {
        if (Runner.IsServer)
        {
            if (_networkPlayerContainer.IsAllReady)
            {
                _enemySpawner.CancelSpawning();
                SceneTransition.TransitioningScene(Runner,SceneName.GameScene);
            }else{
                Debug.Log("Not All Ready");
            }
        }
    }
    void IPlayerJoined.PlayerJoined(PlayerRef player)
    {
        if (Runner.IsServer)
        {
            _networkPlayerSpawner.SpawnPlayer(player,_networkPlayerContainer);
    
            // Todo: RunnerがSetActiveシーンでシーンの切り替えをする時に対応するシーンマネジャーのUniTaskのキャンセルトークンを呼びたい
        }
    }
    
    
    void IPlayerLeft.PlayerLeft(PlayerRef player)
    {
        if (Runner.IsServer)
        {
            _networkPlayerSpawner.DespawnPlayer(player,_networkPlayerContainer);
        }
        
    }
    
}
}
