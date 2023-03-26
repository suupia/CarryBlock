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
            var _ = enemySpawner.StartSimpleSpawner(0, 3f);
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
