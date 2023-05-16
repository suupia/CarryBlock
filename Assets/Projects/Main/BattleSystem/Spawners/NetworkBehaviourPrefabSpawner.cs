using Fusion;
using UnityEngine;
using Enemy;


namespace Main
{
    public interface IPrefabLoader<out T> where T : Object
    {
        T Load(string prefabName);
    }


// Other classes can be created by implementing IPrefabLoader, such as PrefabLoaderFromAssetBundle and PrefabLoaderFromStreamingAssets.
    public class PrefabLoaderFromResources<T> : IPrefabLoader<T> where T : Object
    {
        readonly string _folderPath;

        public PrefabLoaderFromResources(string folderPath)
        {
            _folderPath = folderPath;
        }

        public T Load(string prefabName)
        {
            var result = Resources.Load<T>(_folderPath + "/" + prefabName);
            if (result == null)
            {
                Debug.LogError($"Failed to load prefab. folderPath ={_folderPath+"/"+prefabName} prefabName = {prefabName}");
                return null;
            }
            else
            {
                return result;
            }
        }
    }
    
    public class ComponentPrefabInstantiate<T> where  T : Component
    {
        readonly IPrefabLoader<T> _prefabLoader;
        readonly string _prefabName;

        public ComponentPrefabInstantiate(IPrefabLoader<T> prefabLoader, string prefabName)
        {
            _prefabLoader = prefabLoader;
            _prefabName = prefabName;
        }

        public T InstantiatePrefab(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var prefab = _prefabLoader.Load(_prefabName);
            var gameObj = Object.Instantiate(prefab, position, rotation, parent);
            var component = gameObj.GetComponent<T>();
            if(component == null) Debug.LogError($"Instantiated object does not have component. T = {typeof(T)}");
            return component;
        }
        public T InstantiatePrefab(Transform parent)
        {
            var prefab = _prefabLoader.Load(_prefabName);
            var gameObj = Object.Instantiate(prefab, parent);
            var component = gameObj.GetComponent<T>();
            if(component == null) Debug.LogError($"Instantiated object does not have component. T = {typeof(T)}");
            return component;
        }
    }

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

    // The following classes specifically determine which prefab to spawn
    public class NetworkPlayerPrefabSpawner : IPrefabSpawner<NetworkPlayerController>
    {
        readonly NetworkBehaviourPrefabSpawner<NetworkPlayerController> _playerPrefabPrefabSpawner;

        public NetworkPlayerPrefabSpawner(NetworkRunner runner)
        {
            _playerPrefabPrefabSpawner = new NetworkBehaviourPrefabSpawner<NetworkPlayerController>(runner,
                new PrefabLoaderFromResources<NetworkPlayerController>("Prefabs/Players"), "PlayerController");
        }

        public NetworkPlayerController SpawnPrefab(Vector3 position, Quaternion rotation, PlayerRef playerRef)
        {
            return _playerPrefabPrefabSpawner.SpawnPrefab(position, rotation, playerRef);
        }
    }

    public class LobbyNetworkPlayerPrefabSpawner : IPrefabSpawner<LobbyNetworkPlayerController>
    {
        readonly NetworkBehaviourPrefabSpawner<LobbyNetworkPlayerController> _playerPrefabPrefabSpawner;

        public LobbyNetworkPlayerPrefabSpawner(NetworkRunner runner)
        {
            _playerPrefabPrefabSpawner = new NetworkBehaviourPrefabSpawner<LobbyNetworkPlayerController>(runner,
                new PrefabLoaderFromResources<LobbyNetworkPlayerController>("Prefabs/Players"),
                "LobbyPlayerController");
        }

        public LobbyNetworkPlayerController SpawnPrefab(Vector3 position, Quaternion rotation, PlayerRef playerRef)
        {
            return _playerPrefabPrefabSpawner.SpawnPrefab(position, rotation, playerRef);
        }
    }

    public class NetworkEnemyPrefabSpawner : IPrefabSpawner<NetworkEnemyController>
    {
        readonly NetworkBehaviourPrefabSpawner<NetworkEnemyController> _playerPrefabPrefabSpawner;

        public NetworkEnemyPrefabSpawner(NetworkRunner runner)
        {
            _playerPrefabPrefabSpawner = new NetworkBehaviourPrefabSpawner<NetworkEnemyController>(runner,
                new PrefabLoaderFromResources<NetworkEnemyController>("Prefabs/Enemys"), "Enemy");
        }

        public NetworkEnemyController SpawnPrefab(Vector3 position, Quaternion rotation, PlayerRef playerRef)
        {
            return _playerPrefabPrefabSpawner.SpawnPrefab(position, rotation, playerRef);
        }
    }
}