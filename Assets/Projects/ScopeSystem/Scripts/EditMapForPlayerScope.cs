using VContainer;
using VContainer.Unity;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Spawners.Interfaces;
using Carry.CarrySystem.Spawners.Scripts;
using Carry.EditMapSystem.EditMap.Scripts;
using Carry.UISystem.UI.EditMap;
using Projects.CarrySystem.Item.Scripts;


namespace Carry.EditMapSystem.EditMapForPlayer.Scripts
{
    public class EditMapForPlayerScope: LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Map
            // JsonとEntityGridMapに関する処理
            builder.Register<EntityGridMapBuilderLeaf>(Lifetime.Scoped).As<IEntityGridMapBuilder>();
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
            builder.Register<EditMapGetter>(Lifetime.Scoped).As<IMapGetter>();
            
            // Input
            builder.Register<EditMapBlockAttacher>(Lifetime.Scoped).As<IEditMapBlockAttacher>();
            builder.Register<MemorableEditMapBlockAttacher>(Lifetime.Scoped).WithParameter("capacity", 100);
            builder.Register<CUIHandleNumber>(Lifetime.Scoped);
            builder.Register<AutoSaveManager>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<EditMapForPlayerInput>();
            builder.RegisterComponentInHierarchy<EditMapCUISave>();
            builder.RegisterComponentInHierarchy<EditMapCUILoad>();
            builder.RegisterComponentInHierarchy<EditMapToolCanvas>();
            
                        
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