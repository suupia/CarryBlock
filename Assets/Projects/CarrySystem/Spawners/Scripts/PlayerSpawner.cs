using System.Collections.Generic;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using Fusion;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Spawners.Scripts
{
    public class PlayerSpawner
    {
        readonly NetworkRunner _runner;
        readonly IPlayerBuilder _playerBuilder;
        readonly List<AbstractNetworkPlayerController> _playerControllers = new();

        [Inject]
        public PlayerSpawner(NetworkRunner runner, IPlayerBuilder playerBuilder)
        {
            _runner = runner;
            _playerBuilder = playerBuilder;
        }
        public void RespawnAllPlayer()
        {
            foreach (var player in _runner.ActivePlayers)
            {
                DespawnPlayer(player);
                SpawnPlayer(player);
            }
        }

        public void SpawnPlayer(PlayerRef player)
        {
            Debug.Log("Spawning Player");
            var spawnPosition = new Vector3(0, 5, 0);
            var playerController = _playerBuilder.Build(spawnPosition, Quaternion.identity, player);
            _runner.SetPlayerObject(player, playerController.Object);
            _playerControllers.Add(playerController);
        }

        public void DespawnPlayer(PlayerRef player)
        {
            if (_runner.TryGetPlayerObject(player, out var networkObject))
            {
                var playerController = networkObject.GetComponent<AbstractNetworkPlayerController>();

                _playerControllers.Remove(playerController);
                _runner.Despawn(networkObject);
                _runner.SetPlayerObject(player, null);
            }
        }
    }
    

}