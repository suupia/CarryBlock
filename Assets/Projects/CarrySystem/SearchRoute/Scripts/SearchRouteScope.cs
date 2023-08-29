using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Fusion;
using UnityEngine;
using VContainer;
using VContainer.Unity;
#nullable enable


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
            builder.Register<BlockPresenterPlacer>(Lifetime.Scoped);
            builder.Register<EntityGridMapSwitcher>(Lifetime.Scoped).As<IMapUpdater>();
            
            // SearchRoute
            builder.Register<WaveletSearchBuilder>(Lifetime.Scoped);


            // Initializer
            builder.RegisterComponentInHierarchy<SearchRouteInitializer>();
            
            // Handler
            builder.RegisterComponentInHierarchy<SearchRouteHandler_Net>();
        }

    }
    
}