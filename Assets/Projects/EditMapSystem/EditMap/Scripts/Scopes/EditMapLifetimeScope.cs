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
using Projects.NetworkUtility.NetworkRunnerManager.Scripts;
using Projects.Utility.Scripts;

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class EditMapLifetimeScope: LifetimeScope
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
            builder.Register<TilePresenterAttacher>(Lifetime.Scoped);

            builder.Register<EntityGridMapSaver>(Lifetime.Scoped);
            builder.Register<EditMapUpdater>(Lifetime.Scoped);
            
            // Input
            builder.Register<CUIHandleNumber>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<EditMapInput>();
            builder.RegisterComponentInHierarchy<EditMapCUISave>();
            builder.RegisterComponentInHierarchy<EditMapCUILoad>();
            
            
            // Initializer
            builder.RegisterComponentInHierarchy<EditMapInitializer>();
        }

    }
}