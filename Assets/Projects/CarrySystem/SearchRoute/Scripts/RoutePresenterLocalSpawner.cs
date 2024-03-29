﻿using Carry.CarrySystem.RoutingAlgorithm.Interfaces;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using UnityEngine;

#nullable enable

namespace Projects.CarrySystem.SearchRoute.Scripts
{
    public class RoutePresenterLocalSpawner : IRoutePresenterSpawner
    {
        readonly IPrefabLoader<RoutePresenterLocal> _routePresenterPrefabSpawner;

        public RoutePresenterLocalSpawner()
        {
            _routePresenterPrefabSpawner =
                new PrefabLoaderFromAddressable<RoutePresenterLocal>("Prefabs/Map/RoutePresenterLocal");
        }

        public RoutePresenterLocal SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            var tilePresenter = _routePresenterPrefabSpawner.Load();
            return Object.Instantiate(tilePresenter, position, rotation);
        }
        
        public IRoutePresenter SpawnIRoutePresenter(Vector3 position, Quaternion rotation)
        {
            return SpawnPrefab(position, rotation);
        }
    }
}