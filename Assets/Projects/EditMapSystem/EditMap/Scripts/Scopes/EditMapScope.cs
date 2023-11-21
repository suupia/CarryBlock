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
            builder.Register<EntityGridMapBuilderLeaf>(Lifetime.Scoped).As<IEntityGridMapBuilder>();
            builder.Register<EntityGridMapLoader>(Lifetime.Scoped);
            builder.Register<EntityGridMapDataConverter>(Lifetime.Scoped);  
            builder.Register<EntityGridMapSaver>(Lifetime.Scoped);
            
            // 対応するプレハブをEntityGridMapを元に生成する
            builder.Register<NetworkPlaceablePresenterSpawner>(Lifetime.Scoped).As<IPlaceablePresenterSpawner>();
            builder.Register<PlaceablePresenterBuilder>(Lifetime.Scoped);
            builder.Register<PlaceablePresenterPlacer>(Lifetime.Scoped);
            builder.Register<IWallPresenterSpawner>(container =>
            {
                var randomWallPresenterSpawner = new RandomWallPresenterSpawner();
                randomWallPresenterSpawner.AddSpawner(new WallPresenterNetSpawner(runner));
                randomWallPresenterSpawner.AddSpawner(new WallPresenterNetSpawner1(runner));
                return randomWallPresenterSpawner;
            }, Lifetime.Scoped);
            builder.Register<RandomWallPresenterPlacer>(Lifetime.Scoped);
            builder.Register<GroundPresenterLocalSpawner>(Lifetime.Scoped).As<IGroundPresenterSpawner>();
            builder.Register<GroundPresenterLocalSpawner>(Lifetime.Scoped).As<IGroundPresenterSpawner>();
            builder.Register<RegularGroundPresenterPlacerLocal>(Lifetime.Scoped);
            builder.Register<EditMapPresenterPlacerComposite>(Lifetime.Scoped).As<IPresenterPlacer>();

            // EditMapSwitcher
            builder.Register<EditMapSwitcher>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            
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