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
        readonly IPrefabLoader<TilePresenterNet> _tilePresenterPrefabSpawner;

        public TilePresenterSpawner(NetworkRunner runner)
        {
            _runner = runner;
            _tilePresenterPrefabSpawner =
                new PrefabLoaderFromAddressable<TilePresenterNet>("Prefabs/Map/TilePresenter");
        }

        public TilePresenterNet SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            var tilePresenter = _tilePresenterPrefabSpawner.Load();
            return _runner.Spawn(tilePresenter, position, rotation, PlayerRef.None);
        }
    }

}