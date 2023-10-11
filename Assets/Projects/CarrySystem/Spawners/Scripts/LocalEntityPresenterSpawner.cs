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
    public class LocalEntityPresenterSpawner : IEntityPresenterSpawner
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<EntityPresenterLocal> _tilePresenterPrefabSpawner;

        public LocalEntityPresenterSpawner(NetworkRunner runner)
        {
            _runner = runner;
            _tilePresenterPrefabSpawner =
                new PrefabLoaderFromAddressable<EntityPresenterLocal>("Prefabs/Map/LocalEntityPresenter");
        }

        public IEntityPresenter SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            var tilePresenter = _tilePresenterPrefabSpawner.Load();
            return UnityEngine.Object.Instantiate(tilePresenter, position, rotation);
        }
    }
}