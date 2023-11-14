using Carry.CarrySystem.Map.Scripts;
using Fusion;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.SearchRoute.Scripts
{
    public class RoutePresenterNetSpawner
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<RoutePresenterNet> _routePresenterPrefabSpawner;

        public RoutePresenterNetSpawner(NetworkRunner runner)
        {
            _runner = runner;
            _routePresenterPrefabSpawner =
                new PrefabLoaderFromAddressable<RoutePresenterNet>("Prefabs/Map/RoutePresenterNet");
        }

        public RoutePresenterNet SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            var tilePresenter = _routePresenterPrefabSpawner.Load();
            return _runner.Spawn(tilePresenter, position, rotation, PlayerRef.None);
        }
    }
}