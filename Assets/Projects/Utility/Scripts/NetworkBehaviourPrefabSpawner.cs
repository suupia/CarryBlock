using Fusion;
using Projects.Utility.Scripts;
using Projects.Utility.Interfaces;
using UnityEngine;

namespace Projects.Utility.Scripts
{
    public class NetworkBehaviourPrefabSpawner<T> where T : NetworkBehaviour
    {
        readonly IPrefabLoader<T> _prefabLoader;
        readonly NetworkRunner _runner;

        public NetworkBehaviourPrefabSpawner(NetworkRunner runner, IPrefabLoader<T> prefabLoader)
        {
            _runner = runner;
            _prefabLoader = prefabLoader;
        }

        public T SpawnPrefab(Vector3 position, Quaternion rotation, PlayerRef playerRef)
        {
            var prefab = _prefabLoader.Load();
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