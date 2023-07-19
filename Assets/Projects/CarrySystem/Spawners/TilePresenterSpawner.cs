using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Scripts;
using Fusion;
using Projects.Utility.Scripts;
using UnityEngine;
using UnityEngine.Tilemaps;

#nullable enable
namespace Carry.CarrySystem.Spawners
{
    public class TilePresenterSpawner
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<TilePresenter_Net> _tilePresenterPrefabSpawner;

        public TilePresenterSpawner(NetworkRunner runner)
        {
            _runner = runner;
            _tilePresenterPrefabSpawner =
                new PrefabLoaderFromResources<TilePresenter_Net>("Prefabs/Map", "TilePresenter");
        }

        public TilePresenter_Net SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            var tilePresenter = _tilePresenterPrefabSpawner.Load();
            return _runner.Spawn(tilePresenter, position, rotation, PlayerRef.None);
        }
    }

    public class GroundSpawner
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<NetworkObject> _groundPrefabSpawner;

        public GroundSpawner(NetworkRunner runner)
        {
            _runner = runner;
            _groundPrefabSpawner =
                new PrefabLoaderFromResources<NetworkObject>("Prefabs/Map", "Ground");
        }

        public void SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            var ground = _groundPrefabSpawner.Load();
            _runner.Spawn(ground, position, rotation, PlayerRef.None);
        }
    }

    public class RockSpawner
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<NetworkObject> _rockPrefabSpawner;

        public RockSpawner(NetworkRunner runner)
        {
            _runner = runner;
            _rockPrefabSpawner =
                new PrefabLoaderFromResources<NetworkObject>("Prefabs/Map", "Rock");
        }

        public void SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            var rock = _rockPrefabSpawner.Load();
            _runner.Spawn(rock, position, rotation, PlayerRef.None);
        }
    }
}