using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Scripts;
using Fusion;
using Nuts.Utility.Scripts;
using UnityEngine;
using UnityEngine.Tilemaps;

#nullable enable
namespace Carry.CarrySystem.Spawners
{
    public class TilePresenterSpawner
    {
        readonly string _prefabName = "TilePresenter";
        readonly NetworkBehaviourPrefabSpawner<TilePresenter_Net> _tilePresenterPrefabSpawner;

        public TilePresenterSpawner(NetworkRunner runner)
        {
            _tilePresenterPrefabSpawner = new(runner,
                new PrefabLoaderFromResources<TilePresenter_Net>("Prefabs/Map"), 
                _prefabName);
        }

        public TilePresenter_Net SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            return _tilePresenterPrefabSpawner.SpawnPrefab(position, rotation, PlayerRef.None);
        }
    }
    public class GroundSpawner
    {
        readonly string _prefabName = "Ground";
        readonly NetworkObjectPrefabSpawner _rockPrefabSpawner;

        public GroundSpawner(NetworkRunner runner)
        {
            _rockPrefabSpawner = new(runner,
                new PrefabLoaderFromResources<NetworkObject>("Prefabs/Map"), 
                _prefabName);
        }

        public void SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            _rockPrefabSpawner.SpawnPrefab(position, rotation, PlayerRef.None);
        }
    }
    public class RockSpawner
    {
        readonly string _prefabName = "Rock";
        readonly NetworkObjectPrefabSpawner _rockPrefabSpawner;

        public RockSpawner(NetworkRunner runner)
        {
            _rockPrefabSpawner = new(runner,
                new PrefabLoaderFromResources<NetworkObject>("Prefabs/Map"), 
                _prefabName);
        }

        public void SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            _rockPrefabSpawner.SpawnPrefab(position, rotation, PlayerRef.None);
        }
    }

}