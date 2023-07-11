using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using Carry.CarrySystem.Spawners;
using Fusion;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Nuts.NetworkUtility.NetworkRunnerManager.Scripts;
using Nuts.Utility.Scripts;

namespace  Carry.CarrySystem.CarryScene.Scripts
{
    public sealed class CarrySceneLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // NetworkRunner
            var runner = FindObjectOfType<NetworkRunner>();
            Debug.Log($"NetworkRunner : {runner}");
            builder.RegisterComponent(runner);
            
            
            builder.Register<PrefabLoaderFromResources<CarryPlayerController_Net>>(Lifetime.Singleton)
                .As<IPrefabLoader<CarryPlayerController_Net>>()
                .WithParameter("folderPath", "Prefabs/Players")
                .WithParameter("prefabName", "CarryPlayerController");
            
            // NetworkRunnerに依存するスクリプト
            // builder.Register<CarryPlayerPrefabSpawner>(Lifetime.Scoped).As< IPrefabSpawner<CarryPlayerController_Net>>();

            
            // Player
            builder.Register<CarryPlayerFactory>(Lifetime.Scoped);
            builder.Register<CarryPlayerBuilder>(Lifetime.Scoped);
            builder.Register<CarryPlayerSpawner>(Lifetime.Scoped);

            builder.RegisterComponentInHierarchy<CarryInitializer>();

            // Serverのドメインスクリプト
            // Map
            builder.Register<EntityGridMapLoader>(Lifetime.Singleton);
            builder.Register<TilePresenterBuilder>(Lifetime.Singleton);
            builder.Register<EntityGridMapSwitcher>(Lifetime.Singleton);
            
            // UI
            builder.Register<GameContext>(Lifetime.Singleton);
            builder.Register<FloorTimer>(Lifetime.Singleton);

            // Clientのドメインスクリプト
            // builder.Register<ReturnToMainBaseGauge>(Lifetime.Singleton);
        }
    }

}
