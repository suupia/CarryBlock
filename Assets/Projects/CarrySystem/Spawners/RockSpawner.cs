using Carry.CarrySystem.Player.Scripts;
using Fusion;
using Nuts.Utility.Scripts;
using UnityEngine;
#nullable enable
namespace Carry.CarrySystem.Spawners
{
    public class GroundSpawner
    {
        readonly string _prefabName = "Ground";
        readonly NetworkObjectPrefabSpawner _rockPrefabPrefabSpawner;

        public GroundSpawner(NetworkRunner runner)
        {
            _rockPrefabPrefabSpawner = new(runner,
                new PrefabLoaderFromResources<NetworkObject>("Prefabs/Map"), 
                _prefabName);
        }

        public void SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            _rockPrefabPrefabSpawner.SpawnPrefab(position, rotation, PlayerRef.None);
        }
    }
    public class RockSpawner
    {
        readonly string _prefabName = "Rock";
        readonly NetworkObjectPrefabSpawner _rockPrefabPrefabSpawner;

        public RockSpawner(NetworkRunner runner)
        {
            _rockPrefabPrefabSpawner = new(runner,
                new PrefabLoaderFromResources<NetworkObject>("Prefabs/Map"), 
                _prefabName);
        }

        public void SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            _rockPrefabPrefabSpawner.SpawnPrefab(position, rotation, PlayerRef.None);
        }
    }

}