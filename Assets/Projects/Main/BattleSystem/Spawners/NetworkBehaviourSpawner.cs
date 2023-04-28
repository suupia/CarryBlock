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
    
    public class NetworkBehaviourSpawner<T> where T : NetworkBehaviour
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<T> _prefabLoader;
        readonly string _prefabName;

        public NetworkBehaviourSpawner(NetworkRunner runner, IPrefabLoader<T> prefabLoader, string prefabName)
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
    public class NetworkPlayerPrefabSpawner : IPrefabSpawner<NetworkPlayerController>
    {
        NetworkBehaviourSpawner<NetworkPlayerController> _playerPrefabSpawner;
        public NetworkPlayerPrefabSpawner(NetworkRunner runner)
        {
            _playerPrefabSpawner =  new NetworkBehaviourSpawner<NetworkPlayerController>(runner,
                new PrefabLoaderFromResources<NetworkPlayerController>("Prefabs/Players"), "PlayerController");
        }  
        
        public NetworkPlayerController SpawnPrefab(Vector3 position, Quaternion rotation , PlayerRef playerRef)
        {
            return _playerPrefabSpawner.SpawnPrefab(position, rotation, playerRef);
        }
    }


}

