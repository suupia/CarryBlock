using System.Collections;
using System.Collections.Generic;
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
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Projects.Utility.Scripts;

namespace  Carry.CarrySystem.CarryScene.Scripts
{
    public sealed class CarryLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // このシーンに遷移した時点でNetworkRunnerは存在していると仮定している
            var runner = FindObjectOfType<NetworkRunner>();
            Debug.Log($"NetworkRunner : {runner}");
            builder.RegisterComponent(runner);
            
            // PrefabLoader 
            builder.Register<PrefabLoaderFromResources<CarryPlayerController_Net>>(Lifetime.Scoped)
                .As<IPrefabLoader<CarryPlayerController_Net>>()
                .WithParameter("folderPath", "Prefabs/Players")
                .WithParameter("prefabName", "CarryPlayerController");
            
            builder.Register<PrefabLoaderFromResources<CartControllerNet>>(Lifetime.Scoped)
                .As<IPrefabLoader<CartControllerNet>>()
                .WithParameter("folderPath", "Prefabs/Carts")
                .WithParameter("prefabName", "CartController"); 
            
            // NetworkRunnerに依存するスクリプト

            // Player
            builder.Register<MainCarryPlayerFactory>(Lifetime.Scoped).As<ICarryPlayerFactory>();
            builder.Register<CarryPlayerBuilder>(Lifetime.Scoped).As<IPlayerBuilder>();
            builder.Register<PlayerSpawner>(Lifetime.Scoped);


            // Serverのドメインスクリプト
            // Map
            builder.Register<EntityGridMapLoader>(Lifetime.Scoped);
            builder.Register<TilePresenterBuilder>(Lifetime.Scoped);
            builder.Register<EntityGridMapSwitcher>(Lifetime.Scoped).As<IMapUpdater>();
            
            // Cart
            builder.Register<CartBuilder>(Lifetime.Scoped);
            builder.Register<CartShortestRouteMove>(Lifetime.Scoped);
            builder.Register<WaveletSearchBuilder>(Lifetime.Scoped);
            builder.Register<HoldingBlockObserver>(Lifetime.Scoped);
            builder.Register<ReachRightEdgeChecker>(Lifetime.Scoped);

            // UI
            builder.Register<GameContext>(Lifetime.Scoped);
            builder.Register<FloorTimer.Scripts.FloorTimer>(Lifetime.Scoped);
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
