using Fusion;
using Nuts.Utility.Scripts;
using UnityEngine;

namespace Nuts.Utility.Scripts
{
    public class NetworkBehaviourPrefabSpawner<T> where T : NetworkBehaviour
    {
        readonly IPrefabLoader<T> _prefabLoader;
        readonly string _prefabName;
        readonly NetworkRunner _runner;

        public NetworkBehaviourPrefabSpawner(NetworkRunner runner, IPrefabLoader<T> prefabLoader, string prefabName)
        {
            _runner = runner;
            _prefabLoader = prefabLoader;
            _prefabName = prefabName;
        }

        public T SpawnPrefab(Vector3 position, Quaternion rotation, PlayerRef playerRef)
        {
            var prefab = _prefabLoader.Load(_prefabName);
            var networkObject = _runner.Spawn(prefab, position, rotation, playerRef);
            var networkBehaviour = networkObject.GetComponent<T>();
            if (networkBehaviour == null) Debug.LogError($"Spawned object does not have NetworkBehaviour. T = {typeof(T)}");

            return networkBehaviour;
        }
    }

    public interface IPrefabSpawner<out T> where T : NetworkBehaviour
    {
        // Client does not need to know prefab name
        T SpawnPrefab(Vector3 position, Quaternion rotation, PlayerRef playerRef);
    }




}