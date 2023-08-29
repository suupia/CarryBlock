using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.FloorTimer.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using Carry.CarrySystem.SearchRoute.Scripts;
using Carry.CarrySystem.Spawners;
using Carry.UISystem.UI.CarryScene;
using Fusion;
using Projects.Utility.Scripts;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Carry.CarrySystem.CarryScene.Scripts
{
    public sealed class CarryScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // このシーンに遷移した時点でNetworkRunnerは存在していると仮定している
            var runner = FindObjectOfType<NetworkRunner>();
            Debug.Log($"NetworkRunner : {runner}");
            builder.RegisterComponent(runner);

            // PrefabLoader 
            builder.Register<PrefabLoaderFromAddressable<CarryPlayerControllerNet>>(Lifetime.Scoped)
                .As<IPrefabLoader<CarryPlayerControllerNet>>()
                .WithParameter("path", "Prefabs/Players/CarryPlayerControllerNet");

            builder.Register<PrefabLoaderFromAddressable<CartControllerNet>>(Lifetime.Scoped)
                .As<IPrefabLoader<CartControllerNet>>()
                .WithParameter("path", "Prefabs/Carts/CartControllerNet");

            // NetworkRunnerに依存するスクリプト

            // Player
            builder.Register<MainCarryPlayerFactory>(Lifetime.Scoped).As<ICarryPlayerFactory>();
            builder.Register<BlockBuilder>(Lifetime.Scoped);
            builder.Register<CarryPlayerBuilder>(Lifetime.Scoped).As<IPlayerBuilder>();
            builder.Register<PlayerSpawner>(Lifetime.Scoped);
            builder.Register<CarryPlayerContainer>(Lifetime.Scoped);


            // Serverのドメインスクリプト
            // Map
            builder.Register<MapBuilderMonoDelegate>(Lifetime.Scoped).As<IEntityGridMapBuilder>();
            builder.Register<EntityGridMapLoader>(Lifetime.Scoped);
            builder.Register<BlockPresenterPlacer>(Lifetime.Scoped).As<IBlockPresenterPlacer>();;
            builder.Register<WallPresenterPlacer>(Lifetime.Scoped);
            builder.Register<GroundPresenterPlacer>(Lifetime.Scoped);
            builder.Register<AllPresenterPlacer>(Lifetime.Scoped).As<IPresenterPlacer>();
            builder.Register<EntityGridMapSwitcher>(Lifetime.Scoped).As<IMapUpdater>();

            builder.RegisterComponentInHierarchy<MapKeyDataNet>();

            // Cart
            builder.Register<CartBuilder>(Lifetime.Scoped);
            builder.Register<CartShortestRouteMove>(Lifetime.Scoped);
            builder.Register<WaveletSearchBuilder>(Lifetime.Scoped);
            builder.Register<HoldingBlockObserver>(Lifetime.Scoped);
            builder.Register<ReachRightEdgeChecker>(Lifetime.Scoped);

            // UI
            builder.Register<GameContext>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<FloorTimerNet>();

            // Notifier
            builder.RegisterComponentInHierarchy<CartMovementNotifierNet>();

            // Handler
            builder.RegisterComponentInHierarchy<PlayerNearCartHandlerNet>();

            // Initializer
            builder.RegisterComponentInHierarchy<CarryInitializer>();

            // View
            builder.RegisterComponentInHierarchy<CarrySceneView>();


            // Clientのドメインスクリプト
            // builder.Register<ReturnToMainBaseGauge>(Lifetime.Singleton);
        }
    }
}