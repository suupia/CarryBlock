using Carry.CarrySystem.CarryScene.Scripts;
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
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Carry.ScopeSystem.Scripts
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
            builder.Register<CarryPlayerBuilder>(Lifetime.Scoped).As<IPlayerBuilder>();
            builder.Register<PlayerSpawner>(Lifetime.Scoped);
            builder.Register<CarryPlayerContainer>(Lifetime.Scoped);


            // Map
            // JsonからEntityGridMapを生成する
            builder.Register<EntityGridMapBuilder>(Lifetime.Scoped).As<IEntityGridMapBuilder>();
            builder.Register<EntityGridMapLoader>(Lifetime.Scoped);
            
            // 対応するプレハブをEntityGridMapを元に生成する
            builder.Register<CarryBlockBuilder>(Lifetime.Scoped).As<IBlockBuilder>();
            builder.Register<CarryBlockPresenterPlacer>(Lifetime.Scoped).As<IBlockPresenterPlacer>();;
            builder.Register<WallPresenterPlacer>(Lifetime.Scoped);
            builder.Register<GroundPresenterPlacer>(Lifetime.Scoped);
            builder.Register<CarryPresenterPlacerContainer>(Lifetime.Scoped).As<IPresenterPlacer>();
            
            // どのマップたちを使うかを決める
            builder.RegisterComponentInHierarchy<MapKeyDataSelectorNet>();
            
            // IMapUpdater
            builder.Register<EntityGridMapSwitcher>(Lifetime.Scoped).As<IMapUpdater>();


            // Cart
            builder.Register<CartBuilder>(Lifetime.Scoped);
            builder.Register<CartShortestRouteMove>(Lifetime.Scoped);
            builder.Register<WaveletSearchBuilder>(Lifetime.Scoped);
            builder.Register<HoldingBlockObserver>(Lifetime.Scoped);
            builder.Register<ReachRightEdgeChecker>(Lifetime.Scoped);

            // UI
            builder.RegisterComponentInHierarchy<FloorTimerNet>();

            // Notifier
            builder.RegisterComponentInHierarchy<CartMovementNotifierNet>();

            // Handler
            builder.RegisterComponentInHierarchy<PlayerNearCartHandlerNet>();

            // Initializer
            builder.RegisterComponentInHierarchy<CarryInitializer>();

            // View
            builder.RegisterComponentInHierarchy<PlayingCanvasUINet>();
            builder.RegisterComponentInHierarchy<ResultCanvasUINet>();
            
            // PostEffect
            builder.RegisterComponentInHierarchy<VignetteBlinker>();

            // Clientのドメインスクリプト
            // builder.Register<ReturnToMainBaseGauge>(Lifetime.Singleton);
        }
    }
}