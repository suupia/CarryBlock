using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : NetworkBehaviour
{
    MyFusion.PlayerSpawner playerManager;
    public override void Spawned()
    {
        playerManager = FindObjectOfType<MyFusion.PlayerSpawner>();

        if (Object.HasStateAuthority)
        {
            playerManager.RespawnAllPlayer();
        }
    }
}
