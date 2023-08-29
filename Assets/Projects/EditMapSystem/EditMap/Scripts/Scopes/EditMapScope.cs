using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using Carry.CarrySystem.Spawners;
using Fusion;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Projects.NetworkUtility.NetworkRunnerManager.Scripts;
using Projects.Utility.Scripts;

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class EditMapScope: LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // このシーンに遷移した時点でNetworkRunnerは存在していると仮定している
            var runner = FindObjectOfType<NetworkRunner>();
            Debug.Log($"NetworkRunner : {runner}");
            builder.RegisterComponent(runner);
            
            // Map
            builder.Register<MapBuilderDomain>(Lifetime.Scoped).As<IEntityGridMapBuilder>();
            builder.Register<EntityGridMapLoader>(Lifetime.Scoped);
            builder.Register<BlockPresenterPlacer>(Lifetime.Scoped).As<IBlockPresenterPlacer>();
            builder.Register<WallPresenterPlacer>(Lifetime.Scoped);
            builder.Register<GroundPresenterPlacer>(Lifetime.Scoped);
            builder.Register<AllPresenterPlacer>(Lifetime.Scoped).As<IPresenterPlacer>();


            builder.Register<EntityGridMapSaver>(Lifetime.Scoped);
            builder.Register<BlockPlacer>(Lifetime.Scoped);
            builder.Register<EditMapUpdater>(Lifetime.Scoped).As<IMapUpdater>();
            
            // Input
            builder.Register<CUIHandleNumber>(Lifetime.Scoped);
            builder.Register<AutoSaveManager>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<EditMapInput>();
            builder.RegisterComponentInHierarchy<EditMapCUISave>();
            builder.RegisterComponentInHierarchy<EditMapCUILoad>();
            
            // Presenter
            builder.RegisterComponentInHierarchy<LoadedFilePresenter>();
            
            
            // Initializer
            builder.RegisterComponentInHierarchy<EditMapInitializer>();
        }

    }
}