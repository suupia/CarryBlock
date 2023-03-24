using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : NetworkBehaviour
{
    MyFusion.PlayerSpawner playerSpawner;
    public override void Spawned()
    {
        playerSpawner = FindObjectOfType<MyFusion.PlayerSpawner>();

        if (Object.HasStateAuthority)
        {
            playerSpawner.RespawnAllPlayer();
        }
    }
}
