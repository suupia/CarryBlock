using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using Carry.CarrySystem.SearchRoute.Scripts;
using Carry.CarrySystem.Spawners;
using Fusion;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Projects.NetworkUtility.NetworkRunnerManager.Scripts;
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
            
            
            builder.Register<PrefabLoaderFromResources<CarryPlayerController_Net>>(Lifetime.Scoped)
                .As<IPrefabLoader<CarryPlayerController_Net>>()
                .WithParameter("folderPath", "Prefabs/Players")
                .WithParameter("prefabName", "CarryPlayerController");
            
            // NetworkRunnerに依存するスクリプト

            // Player
            builder.Register<DefaultCarryPlayerFactory>(Lifetime.Scoped).As<ICarryPlayerFactory>();
            builder.Register<CarryPlayerBuilder>(Lifetime.Scoped);
            builder.Register<CarryPlayerSpawner>(Lifetime.Scoped);


            // Serverのドメインスクリプト
            // Map
            builder.Register<EntityGridMapLoader>(Lifetime.Scoped);
            builder.Register<TilePresenterBuilder>(Lifetime.Scoped);
            builder.Register<EntityGridMapSwitcher>(Lifetime.Scoped).As<IMapUpdater>();
            
            // SearchRoute
            builder.Register<WaveletSearchBuilder>(Lifetime.Scoped);
            builder.Register<HoldingBlockObserver>(Lifetime.Scoped);
            
            // UI
            builder.Register<GameContext>(Lifetime.Scoped);
            builder.Register<FloorTimer>(Lifetime.Scoped);
            
            // Initializer
            builder.RegisterComponentInHierarchy<CarryInitializer>();
            
            // Notifier
            builder.RegisterComponentInHierarchy<CartMovementNotifier>();


            // Clientのドメインスクリプト
            // builder.Register<ReturnToMainBaseGauge>(Lifetime.Singleton);
        }
    }

}
