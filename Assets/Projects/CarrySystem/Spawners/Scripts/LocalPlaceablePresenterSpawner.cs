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
    public class LocalPlaceablePresenterSpawner : IPlaceablePresenterSpawner
    {
        readonly IPrefabLoader<PlaceablePresenterLocal> _tilePresenterPrefabSpawner;

        public LocalPlaceablePresenterSpawner()
        {
            _tilePresenterPrefabSpawner =
                new PrefabLoaderFromAddressable<PlaceablePresenterLocal>("Prefabs/Map/PlaceablePresenterLocal");
        }

        public IPlaceablePresenter SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            var tilePresenter = _tilePresenterPrefabSpawner.Load();
            return UnityEngine.Object.Instantiate(tilePresenter, position, rotation);
        }
    }
}