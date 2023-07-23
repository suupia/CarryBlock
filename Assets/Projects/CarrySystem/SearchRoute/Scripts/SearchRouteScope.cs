using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Fusion;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Carry.CarrySystem.SearchRoute.Scripts
{
    public class SearchRouteScope: LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // このシーンに遷移した時点でNetworkRunnerは存在していると仮定している
            var runner = FindObjectOfType<NetworkRunner>();
            Debug.Log($"NetworkRunner : {runner}");
            builder.RegisterComponent(runner);
            
            // Map
            builder.Register<EntityGridMapLoader>(Lifetime.Scoped);
            builder.Register<TilePresenterBuilder>(Lifetime.Scoped);
            builder.Register<EntityGridMapSwitcher>(Lifetime.Scoped).As<IMapUpdater>();
            
            // SearchRoute
            builder.Register<SearchShortestRoute>(Lifetime.Scoped);
            builder.Register<RoutePresenterBuilder>(Lifetime.Scoped);
            builder.Register<SearchRouteUpdater>(Lifetime.Scoped);


            // Initializer
            builder.RegisterComponentInHierarchy<SearchRouteInitializer>();
            
            // Handler
            builder.RegisterComponentInHierarchy<SearchRouteHandler_Net>();
        }

    }
    
}