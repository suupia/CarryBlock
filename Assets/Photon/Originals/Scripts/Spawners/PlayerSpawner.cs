using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFusion
{
    public class PlayerSpawner : SimulationBehaviour
    {
        [SerializeField] NetworkPrefabRef playerPrefab;

        List<PlayerController> playerControllers = new();

        public PlayerController[] PlayerControllers => playerControllers.ToArray();

        //Will be called outer NetworkBehaviour
        public void RespawnAllPlayer()
        {
            foreach (var player in Runner.ActivePlayers)
            {
                DespawnPlayer(player);
                SpawnPlayer(player);
            }
        }

        public void SpawnPlayer(PlayerRef player)
        {
            Debug.Log("Spawning Player");
            var spawnPosition = new Vector3(0, 1, 0);
            var playerObject = Runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);
            Runner.SetPlayerObject(player, playerObject);
            //TODO: Set AoI

            playerControllers.Add(playerObject.GetComponent<PlayerController>());
        }

        public void DespawnPlayer(PlayerRef player)
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
