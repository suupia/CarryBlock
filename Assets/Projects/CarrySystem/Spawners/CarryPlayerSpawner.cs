using System.Collections.Generic;
using Carry.CarrySystem.Player.Scripts;
using Fusion;
using Nuts.Utility.Scripts;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Spawners
{
    public class CarryPlayerSpawner
    {
        readonly NetworkRunner _runner;
        readonly CarryPlayerBuilder _carryPlayerBuilder;
        readonly List<CarryPlayerController_Net> _playerControllers = new();

        [Inject]
        public CarryPlayerSpawner(NetworkRunner runner, CarryPlayerBuilder carryPlayerBuilder)
        {
            Debug.Log($"CarryPlayerSpawner Constructor");

            _runner = runner;
            _carryPlayerBuilder = carryPlayerBuilder;
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
            var playerController = _carryPlayerBuilder.Build(PlayerColorType.Red, spawnPosition, Quaternion.identity, player);
            _runner.SetPlayerObject(player, playerController.Object);
            _playerControllers.Add(playerController);
        }

        public void DespawnPlayer(PlayerRef player)
        {
            if (_runner.TryGetPlayerObject(player, out var networkObject))
            {
                var playerController = networkObject.GetComponent<CarryPlayerController_Net>();

                _playerControllers.Remove(playerController);
                _runner.Despawn(networkObject);
                _runner.SetPlayerObject(player, null);
            }
        }
    }
    

}