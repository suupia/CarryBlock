using Carry.CarrySystem.Player.Scripts;
using Fusion;
using Nuts.Utility.Scripts;
using UnityEngine;
#nullable enable
namespace Carry.CarrySystem.Spawners
{
    public class RockSpawner
    {
        readonly NetworkRunner _runner;
        readonly string _prefabName = "Ground";
        NetworkObjectPrefabSpawner _groundPrefabPrefabSpawner;

        public RockSpawner(NetworkRunner runner)
        {
            _runner = runner;
            _groundPrefabPrefabSpawner = new(_runner,
                new PrefabLoaderFromResources<NetworkObject>("Prefabs/Map"), 
                _prefabName);
        }

        public void SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            _groundPrefabPrefabSpawner.SpawnPrefab(position, rotation, PlayerRef.None);
        }
    }

}