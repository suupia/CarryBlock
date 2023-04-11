using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GameInitializer : NetworkSceneInitializer
{
    async void Start()
    {
        await runnerManager.StartScene("GameSceneTestRoom");
        base.Init();
        await UniTask.WaitUntil(() => Runner.SceneManager.IsReady(Runner), cancellationToken: token); 
        
        if (Runner.IsServer)
        {
            playerSpawner.RespawnAllPlayer(networkPlayerContainer);
        }

        if (Runner.IsServer)
        {
            networkEnemyContainer.MaxEnemyCount = 5;
            var _ = enemySpawner.StartSimpleSpawner(0, 5f,networkEnemyContainer);
        }
    }
    
    // Return to LobbyScene
    public void SetActiveLobbyScene()
    {
        if (Runner.IsServer)
        {
            phaseManager.SetPhase(Phase.Ending);
            networkEnemyContainer.MaxEnemyCount = 128;
        }
    }
    
 
}
