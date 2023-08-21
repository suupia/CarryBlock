using Carry.CarrySystem.Map.Scripts;
using Fusion;
using Projects.Utility.Scripts;
using UnityEngine;

namespace Carry.CarrySystem.Spawners
{
    public class GroundPresenterSpawner
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<GroundPresenterNet> _tilePresenterPrefabSpawner;

        public GroundPresenterSpawner(NetworkRunner runner)
        {
            _runner = runner;
            _tilePresenterPrefabSpawner =
                new PrefabLoaderFromAddressable<GroundPresenterNet>("Prefabs/Map/GroundPresenter");
        }

        public GroundPresenterNet SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            var tilePresenter = _tilePresenterPrefabSpawner.Load();
            return _runner.Spawn(tilePresenter, position, rotation, PlayerRef.None);
        }
    }
}