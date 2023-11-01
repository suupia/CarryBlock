using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Spawners.Interfaces;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using Fusion;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Spawners.Scripts
{
    public class EntityPresenterSpawner : IEntityPresenterSpawner
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<EntityPresenterNet> _tilePresenterPrefabSpawner;

        public EntityPresenterSpawner(NetworkRunner runner)
        {
            _runner = runner;
            _tilePresenterPrefabSpawner =
                new PrefabLoaderFromAddressable<EntityPresenterNet>("Prefabs/Map/EntityPresenter");
        }

        public IEntityPresenter SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            var tilePresenter = _tilePresenterPrefabSpawner.Load();
            return _runner.Spawn(tilePresenter, position, rotation, PlayerRef.None);
        }
        public EntityPresenterNet SpawnPrefabNet(Vector3 position, Quaternion rotation)
        {
            var tilePresenter = _tilePresenterPrefabSpawner.Load();
            Debug.Log("SpawnPrefabNet");
            return _runner.Spawn(tilePresenter, position, rotation, PlayerRef.None);
        }
    }
}