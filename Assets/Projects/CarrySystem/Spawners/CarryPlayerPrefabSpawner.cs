using Carry.CarrySystem.Player.Scripts;
using Fusion;
using Projects.Utility.Scripts;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Spawners
{
    public class CarryPlayerPrefabSpawner : IPrefabSpawner<CarryPlayerControllerNet>
    {
        readonly NetworkBehaviourPrefabSpawner<CarryPlayerControllerNet> _playerPrefabPrefabSpawner;

        [Inject]
        public CarryPlayerPrefabSpawner(NetworkRunner runner)
        {
            _playerPrefabPrefabSpawner = new NetworkBehaviourPrefabSpawner<CarryPlayerControllerNet>(runner,
                new PrefabLoaderFromAddressable<CarryPlayerControllerNet>("Prefabs/Players/CarryPlayerControllerNet"));
        }

        public CarryPlayerControllerNet SpawnPrefab(Vector3 position, Quaternion rotation, PlayerRef playerRef)
        {
            return _playerPrefabPrefabSpawner.SpawnPrefab(position, rotation, playerRef);
        }
    }
}