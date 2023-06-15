using Fusion;
using Nuts.Utility.Scripts;
using UnityEngine;

namespace Nuts.Utility.Scripts
{
    public class NetworkObjectPrefabSpawner
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<NetworkObject> _prefabLoader;
        readonly string _prefabName;

        public NetworkObjectPrefabSpawner(NetworkRunner runner, IPrefabLoader<NetworkObject> prefabLoader, string prefabName)
        {
            _runner = runner;
            _prefabLoader = prefabLoader;
            _prefabName = prefabName;
        }

        public NetworkObject SpawnPrefab(Vector3 position, Quaternion rotation, PlayerRef playerRef)
        {
            var prefab = _prefabLoader.Load(_prefabName);
            var networkObject = _runner.Spawn(prefab, position, rotation, playerRef);
            if (networkObject == null) Debug.LogError($"Spawned object does not have NetworkObject.");

            return networkObject;
        }

    }
}