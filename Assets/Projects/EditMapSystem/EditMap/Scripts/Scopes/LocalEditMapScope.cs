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
using Carry.NetworkUtility.NetworkRunnerManager.Scripts;
using Carry.Utility.Scripts;
using Projects.CarrySystem.Item.Scripts;

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class LocalEditMapScope: LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // // このシーンではNetworkRunnerは使用しない！
            // var runner = FindObjectOfType<NetworkRunner>();
            // Debug.Log($"NetworkRunner : {runner}"); 
            // builder.RegisterComponent(runner);
            
            // Map
            // JsonとEntityGridMapに関する処理
            builder.Register<EntityGridMapBuilder>(Lifetime.Scoped);
            builder.Register<EntityGridMapLoader>(Lifetime.Scoped);
            builder.Register<EntityGridMapSaver>(Lifetime.Scoped);
            
            // 対応するプレハブをEntityGridMapを元に生成する
            builder.Register<EditMapBlockBuilder>(Lifetime.Scoped);
            builder.Register<EditMapBlockPresenterPlacer>(Lifetime.Scoped);
            builder.Register<RandomWallPresenterPlacer>(Lifetime.Scoped);
            builder.Register<RegularGroundPresenterPlacer>(Lifetime.Scoped);
            builder.Register<EditMapPresenterPlacerContainer>(Lifetime.Scoped).As<IPresenterPlacer>();

            // IMapUpdater
            builder.Register<EditMapUpdater>(Lifetime.Scoped).As<IMapUpdater>();
            
            // Input
            builder.Register<EditMapBlockAttacher>(Lifetime.Scoped);
            builder.Register<CUIHandleNumber>(Lifetime.Scoped);
            builder.Register<AutoSaveManager>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<EditMapInput>();
            builder.RegisterComponentInHierarchy<EditMapCUISave>();
            builder.RegisterComponentInHierarchy<EditMapCUILoad>();
            
                        
            //Item
            builder.Register<TreasureCoinCounter>(Lifetime.Scoped);
            
            // Presenter
            builder.RegisterComponentInHierarchy<LoadedFilePresenter>();
            
            
            // Initializer
            builder.RegisterComponentInHierarchy<LocalEditMapInitializer>();
        }

    }
}