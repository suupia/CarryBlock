
#nullable enable
using System.Collections.Generic;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using Carry.CarrySystem.Player.Scripts.Local;
using Fusion;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Spawners.Scripts
{
    public class LocalPlayerSpawner
    {
        readonly CarryPlayerControllerLocalBuilder _playerControllerLocalBuilder;
        readonly List<CarryPlayerControllerLocal> _playerControllers = new();

        [Inject]
        public LocalPlayerSpawner(CarryPlayerControllerLocalBuilder playerControllerLocalBuilder)
        {
            _playerControllerLocalBuilder = playerControllerLocalBuilder;
        }
        
        public void RespawnAllPlayer()
        {
            // todo : たぶんローカルだと必要ないので、使わなかったら削除
            // foreach (var player in _runner.ActivePlayers)
            // {
            //     DespawnPlayer(player);
            //     SpawnPlayer(player);
            // }
        }

        public void SpawnPlayer()
        {
            Debug.Log("LocalPlayerSpawner SpawnPlayer()");
            var spawnPosition = new Vector3(0, 5, 0);
            var playerController = _playerControllerLocalBuilder.Build(spawnPosition, Quaternion.identity);
            _playerControllers.Add(playerController);
        }

        public void DespawnPlayer()
        {
            foreach (var carryPlayerControllerLocal in _playerControllers)
            {
                Object.Destroy(carryPlayerControllerLocal.gameObject);
            }
            _playerControllers.Clear();
        }
    }
}