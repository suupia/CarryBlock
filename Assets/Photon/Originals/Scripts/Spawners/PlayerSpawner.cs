using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFusion
{
    public class PlayerSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft
    {
        [SerializeField] NetworkPrefabRef playerPrefab;

        List<PlayerController> playerControllers = new();

        public PlayerController[] PlayerControllers => playerControllers.ToArray();

        public void PlayerJoined(PlayerRef player)
        {
            if (Runner.IsServer)
            {
                SpawnPlayer(player);
            }
        }

        public void PlayerLeft(PlayerRef player)
        {
            if (Runner.IsServer)
            {
                DespawnPlayer(player);
            }
        }

        //Will be called outer NetworkBehaviour
        public void RespawnAllPlayer()
        {
            foreach (var player in Runner.ActivePlayers)
            {
                DespawnPlayer(player);
                SpawnPlayer(player);
            }
        }

        void SpawnPlayer(PlayerRef player)
        {
            Debug.Log("Spawning Player");
            var spawnPosition = new Vector3(0, 1, 0);
            var playerObject = Runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);
            Runner.SetPlayerObject(player, playerObject);
            //TODO: Set AoI

            playerControllers.Add(playerObject.GetComponent<PlayerController>());
        }

        void DespawnPlayer(PlayerRef player)
        {
            if (Runner.TryGetPlayerObject(player, out var networkObject))
            {
                var pc = networkObject.GetComponent<PlayerController>();
                playerControllers.Remove(pc);
                Runner.Despawn(networkObject);
                Runner.SetPlayerObject(player, null);
            }
        }
    }
}
