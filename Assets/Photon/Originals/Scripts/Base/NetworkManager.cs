using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkManager : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    protected MyFusion.PlayerSpawner playerSpawner;
    protected MyFusion.EnemySpawner enemySpawner;
    protected PhaseManager phaseManager;

    public override void Spawned()
    {
        playerSpawner = FindObjectOfType<MyFusion.PlayerSpawner>();
        enemySpawner = FindObjectOfType<MyFusion.EnemySpawner>();
        phaseManager = FindObjectOfType<PhaseManager>();


        if (Object.HasStateAuthority)
        {
            playerSpawner.RespawnAllPlayer();
        }
    }

    public override void FixedUpdateNetwork()
    {
        //��ŃL���b�V�������悤�ɂ��ē�����P���A���������̎d�l��ς���
        var playerUnits = playerSpawner.PlayerControllers.Map(pc => pc.NowUnit);
        Array.ForEach(enemySpawner.Enemies, e => e.SetDirection(playerUnits));
    }

    public void PlayerJoined(PlayerRef player)
    {
        if (Runner.IsServer)
        {
            playerSpawner.SpawnPlayer(player);
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (Runner.IsServer)
        {
            playerSpawner.DespawnPlayer(player);
        }
    }

}
