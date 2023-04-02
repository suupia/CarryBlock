using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LobbyManager : NetworkManager
{
    public override void Spawned()
    {
        base.Spawned();
        if (Object.HasStateAuthority)
        {
            enemySpawner.MaxEnemyCount = 5;
            var _ = enemySpawner.StartSimpleSpawner(0, 5f);
        }
    }

    public void SetActiveGameScene()
    {
        if (Object.HasStateAuthority)
        {
            if (playerSpawner.IsAllReady)
            {
                phaseManager.SetPhase(Phase.Starting);
            }
        }
    }
}
