using Carry.CarrySystem.Map.Scripts;
using Fusion;
using Projects.Utility.Scripts;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.SearchRoute.Scripts
{
    public class RoutePresenterSpawner
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<RoutePresenter_Net> _routePresenterPrefabSpawner;

        public RoutePresenterSpawner(NetworkRunner runner)
        {
            _runner = runner;
            _routePresenterPrefabSpawner =
                new PrefabLoaderFromResources<RoutePresenter_Net>("Prefabs/Map", "RoutePresenter");
        }

        public RoutePresenter_Net SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            var tilePresenter = _routePresenterPrefabSpawner.Load();
            return _runner.Spawn(tilePresenter, position, rotation, PlayerRef.None);
        }
    }
}