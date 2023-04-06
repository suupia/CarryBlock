using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : NetworkSceneInitializer
{
    async void Start()
    {
        await base.Init();

    }
    public void SetActiveLobbyScene()
    {
        if (Runner.IsServer)
        {
            phaseManager.SetPhase(Phase.Ending);
            networkEnemyContainer.MaxEnemyCount = 128;
        }
    }
}
