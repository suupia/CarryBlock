using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Spawners.Interfaces;
using Carry.CarrySystem.Spawners.Scripts;
using Carry.EditMapSystem.EditMap.Scripts;
using VContainer;
using VContainer.Unity;
using Projects.CarrySystem.Item.Scripts;

namespace Carry.EditMapSystem.EditMapForPlayer.Scripts
{
    public class EditMapForPlayerScope: LifetimeScope
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
            builder.Register<LocalEntityPresenterSpawner>(Lifetime.Scoped).As<IEntityPresenterSpawner>();
            builder.Register<EditMapBlockBuilder>(Lifetime.Scoped);
            builder.Register<RandomWallPresenterPlacerLocal>(Lifetime.Scoped);
            builder.Register<RegularGroundPresenterPlacerLocal>(Lifetime.Scoped);
            builder.Register<EditMapBlockPresenterPlacer>(Lifetime.Scoped);
            builder.Register<LocalEditMapPresenterPlacerComponent>(Lifetime.Scoped).As<IPresenterPlacer>();

            // IMapUpdater
            builder.Register<EditMapUpdater>(Lifetime.Scoped).As<IMapUpdater>();
            
            // Input
            builder.Register<EditMapBlockAttacher>(Lifetime.Scoped);
            builder.Register<CUIHandleNumber>(Lifetime.Scoped);
            builder.Register<AutoSaveManager>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<EditMapForPlayerInput>();
            builder.RegisterComponentInHierarchy<EditMapCUISave>();
            builder.RegisterComponentInHierarchy<EditMapCUILoad>();
            
                        
            //Item
            builder.Register<TreasureCoinCounter>(Lifetime.Scoped);
            
            // Presenter
            builder.RegisterComponentInHierarchy<LoadedFilePresenter>();
            
            // MapKey
            builder.RegisterComponentInHierarchy<MapKeyContainer>();
            
            // Initializer
            builder.RegisterComponentInHierarchy<LocalEditMapInitializer>();
        }

    }
}