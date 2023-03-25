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
        
    }

    public void SetActiveGameScene()
    {
        if (Object.HasStateAuthority)
        {
            var canStartGame = playerSpawner.PlayerControllers.All(pc => pc.IsReady);

            if (canStartGame)
            {
                phaseManager.SetPhase(Phase.Starting);
            }
        }
    }
}
