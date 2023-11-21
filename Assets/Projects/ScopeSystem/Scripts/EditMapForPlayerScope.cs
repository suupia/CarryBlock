using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Spawners.Interfaces;
using Carry.CarrySystem.Spawners.Scripts;
using Carry.EditMapSystem.EditMap.Scripts;
using Carry.UISystem.UI.EditMap;
using Projects.CarrySystem.Item.Scripts;
using VContainer;
using VContainer.Unity;

namespace Carry.EditMapSystem.EditMapForPlayer.Scripts
{
    public class EditMapForPlayerScope: LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Map
            // JsonとEntityGridMapに関する処理
            builder.Register<EntityGridMapBuilderLeaf>(Lifetime.Scoped).As<IEntityGridMapBuilder>();
            builder.Register<EntityGridMapDataConverter>(Lifetime.Scoped);
            builder.Register<EntityGridMapLoader>(Lifetime.Scoped);
            builder.Register<EntityGridMapSaver>(Lifetime.Scoped);
            
            // 対応するプレハブをEntityGridMapを元に生成する
            builder.Register<LocalPlaceablePresenterSpawner>(Lifetime.Scoped).As<IPlaceablePresenterSpawner>();
            builder.Register<PlaceablePresenterBuilder>(Lifetime.Scoped);
            builder.Register<IWallPresenterSpawner>(container =>
            { 
                var randomWallPresenterSpawner = new RandomWallPresenterSpawner();
                randomWallPresenterSpawner.AddSpawner(new WallPresenterLocalSpawner());
                randomWallPresenterSpawner.AddSpawner(new WallPresenterLocalSpawner1());
                return randomWallPresenterSpawner;
            }, Lifetime.Scoped);
            builder.Register<RandomWallPresenterPlacer>(Lifetime.Scoped);
            builder.Register<GroundPresenterLocalSpawner>(Lifetime.Scoped).As<IGroundPresenterSpawner>();
            builder.Register<GroundPresenterPlacer>(Lifetime.Scoped);
            builder.Register<PlaceablePresenterPlacer>(Lifetime.Scoped);
            builder.Register<EditMapPresenterPlacerComposite>(Lifetime.Scoped).As<IPresenterPlacer>();

            // EditMapSwitcher
            builder.Register<EditMapSwitcher>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            
            // Input
            builder.Register<EditMapBlockAttacher>(Lifetime.Scoped).As<IEditMapBlockAttacher>();
            builder.Register<MemorableEditMapBlockAttacher>(Lifetime.Scoped).WithParameter("capacity", 100);
            builder.Register<CUIHandleNumber>(Lifetime.Scoped);
            builder.Register<AutoSaveManager>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<EditMapForPlayerInput>();
            builder.RegisterComponentInHierarchy<EditMapCUISave>();
            builder.RegisterComponentInHierarchy<EditMapCUILoad>();
            
            // UI
            builder.RegisterComponentInHierarchy<MapMakerToolCanvas>();
            
                        
            //Item
            builder.Register<TreasureCoinCounter>(Lifetime.Scoped);
            
            // Presenter
            builder.RegisterComponentInHierarchy<LoadedFilePresenter>();
            
            // MapKey
            builder.RegisterComponentInHierarchy<MapKeyContainer>();
            
            // Initializer
            builder.RegisterComponentInHierarchy<EditMapForPlayerInitializer>();
        }

    }
}