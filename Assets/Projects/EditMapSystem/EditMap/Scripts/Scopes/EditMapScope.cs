using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using Carry.CarrySystem.Spawners.Interfaces;
using Carry.CarrySystem.Spawners.Scripts;
using Fusion;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Carry.NetworkUtility.NetworkRunnerManager.Scripts;
using Carry.Utility.Scripts;
using Projects.CarrySystem.Item.Scripts;

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

            var mapKeyContainer = FindObjectOfType<MapKeyContainer>();
            Debug.Log($"MapKeyContainer : {mapKeyContainer}");
            builder.RegisterComponent(mapKeyContainer);
            
            // Map
            // JsonとEntityGridMapに関する処理
            builder.Register<EntityGridMapBuilder>(Lifetime.Scoped);
            builder.Register<EntityGridMapLoader>(Lifetime.Scoped);
            builder.Register<EntityGridMapSaver>(Lifetime.Scoped);
            
            // 対応するプレハブをEntityGridMapを元に生成する
            builder.Register<EntityPresenterSpawner>(Lifetime.Scoped).As<IEntityPresenterSpawner>();
            builder.Register<EditMapBlockBuilder>(Lifetime.Scoped);
            builder.Register<EditMapBlockPresenterPlacer>(Lifetime.Scoped);
            builder.Register<RandomWallPresenterPlacerNet>(Lifetime.Scoped);
            builder.Register<RegularGroundPresenterPlacerLocal>(Lifetime.Scoped);
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
            builder.RegisterComponentInHierarchy<EditMapInitializer>();
        }

    }
}