using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

namespace Main
{
    public interface IPrefabLoader<out T> where T : UnityEngine.Object
    {
        T Load(string prefabName);
    }


// Other classes can be created by implementing IPrefabLoader, such as PrefabLoaderFromAssetBundle and PrefabLoaderFromStreamingAssets.
    public class PrefabLoaderFromResources<T>: IPrefabLoader<T> where T : UnityEngine.Object
    {
        readonly string _folderPath;

        public PrefabLoaderFromResources(string folderPath)
        {
            _folderPath = folderPath;
        }

        public T Load(string prefabName)
        {
            return Resources.Load<T>(_folderPath + "/" + prefabName);
        }
    }
    
    public class NetworkBehaviourPrefabSpawner<T> where T : NetworkBehaviour
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<T> _prefabLoader;
        readonly string _prefabName;

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
            if (networkBehaviour == null)
            {
                Debug.LogError("Spawned object does not have NetworkBehaviour");
            }

            return networkBehaviour;
        }
    }

    public interface IPrefabSpawner<out T> where T : NetworkBehaviour
    {
        // Client does not need to know prefab name
        T SpawnPrefab(Vector3 position, Quaternion rotation, PlayerRef playerRef);
    }
    
    // The following classes specifically determine which prefab to spawn
    public class NetworkPlayerPrefabSpawner : IPrefabSpawner<NetworkPlayerController>
    {
        NetworkBehaviourPrefabSpawner<NetworkPlayerController> _playerPrefabPrefabSpawner;
        public NetworkPlayerPrefabSpawner(NetworkRunner runner)
        {
            _playerPrefabPrefabSpawner =  new NetworkBehaviourPrefabSpawner<NetworkPlayerController>(runner,
                new PrefabLoaderFromResources<NetworkPlayerController>("Prefabs/Players"), "PlayerController");
        }  
        
        public NetworkPlayerController SpawnPrefab(Vector3 position, Quaternion rotation , PlayerRef playerRef)
        {
            return _playerPrefabPrefabSpawner.SpawnPrefab(position, rotation, playerRef);
        }
    }
    
    public class NetworkEnemyPrefabSpawner : IPrefabSpawner<NetworkEnemyController>
    {
        NetworkBehaviourPrefabSpawner<NetworkEnemyController> _playerPrefabPrefabSpawner;
        public NetworkEnemyPrefabSpawner(NetworkRunner runner)
        {
            _playerPrefabPrefabSpawner =  new NetworkBehaviourPrefabSpawner<NetworkEnemyController>(runner,
                new PrefabLoaderFromResources<NetworkEnemyController>("Prefabs/Enemys"), "Enemy");
        }  
        
        public NetworkEnemyController SpawnPrefab(Vector3 position, Quaternion rotation , PlayerRef playerRef)
        {
            return _playerPrefabPrefabSpawner.SpawnPrefab(position, rotation, playerRef);
        }
    }

}

