using Carry.CarrySystem.Player.Scripts;
using Fusion;
using Projects.Utility.Scripts;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Spawners
{
    public class CarryPlayerPrefabSpawner : IPrefabSpawner<CarryPlayerController_Net>
    {
        readonly NetworkBehaviourPrefabSpawner<CarryPlayerController_Net> _playerPrefabPrefabSpawner;

        [Inject]
        public CarryPlayerPrefabSpawner(NetworkRunner runner)
        {
            _playerPrefabPrefabSpawner = new NetworkBehaviourPrefabSpawner<CarryPlayerController_Net>(runner,
                new PrefabLoaderFromAddressable<CarryPlayerController_Net>("Prefabs/Players/CarryPlayerController"));
        }

        public CarryPlayerController_Net SpawnPrefab(Vector3 position, Quaternion rotation, PlayerRef playerRef)
        {
            return _playerPrefabPrefabSpawner.SpawnPrefab(position, rotation, playerRef);
        }
    }
}