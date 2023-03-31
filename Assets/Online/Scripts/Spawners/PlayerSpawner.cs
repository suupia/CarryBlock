using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyFusion
{
    public class PlayerSpawner : SimulationBehaviour
    {
        [SerializeField] PlayerController playerPrefab;

        List<PlayerController> playerControllers = new();

        public PlayerController[] PlayerControllers => playerControllers.ToArray();
        public bool IsAllReady => PlayerControllers.All(pc => pc.IsReady);

        //Will be called outer NetworkBehaviour
        public void RespawnAllPlayer()
        {
            if (Runner == null)
            {
                Runner = FindObjectOfType<NetworkRunner>();
            }
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
            Runner.SetPlayerObject(player, playerObject.Object);
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
