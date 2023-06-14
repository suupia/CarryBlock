using Carry.CarrySystem.Player.Scripts;
using Fusion;
using Nuts.Utility.Scripts;
using UnityEngine;

namespace Carry.CarrySystem.Spawners
{
    public class CarryPlayerPrefabSpawner : IPrefabSpawner<CarryPlayerController_Net>
    {
        readonly NetworkBehaviourPrefabSpawner<CarryPlayerController_Net> _playerPrefabPrefabSpawner;

        public CarryPlayerPrefabSpawner(NetworkRunner runner)
        {
            _playerPrefabPrefabSpawner = new NetworkBehaviourPrefabSpawner<CarryPlayerController_Net>(runner,
                new PrefabLoaderFromResources<CarryPlayerController_Net>("Prefabs/Players"), "CarryPlayerController");
        }

        public CarryPlayerController_Net SpawnPrefab(Vector3 position, Quaternion rotation, PlayerRef playerRef)
        {
            return _playerPrefabPrefabSpawner.SpawnPrefab(position, rotation, playerRef);
        }
    }
}