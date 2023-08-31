using Carry.CarrySystem.Map.Scripts;
using Fusion;
using Projects.Utility.Interfaces;
using Projects.Utility.Scripts;
using UnityEngine;

namespace Carry.CarrySystem.Spawners
{
    public class WallPresenterSpawner
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<WallPresenterNet> _tilePresenterPrefabSpawner;

        public WallPresenterSpawner(NetworkRunner runner)
        {
            _runner = runner;
            _tilePresenterPrefabSpawner =
                new PrefabLoaderFromAddressable<WallPresenterNet>("Prefabs/Map/WallPresenter");
        }

        public WallPresenterNet SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            var tilePresenter = _tilePresenterPrefabSpawner.Load();
            return _runner.Spawn(tilePresenter, position, rotation, PlayerRef.None);
        }
    }
}