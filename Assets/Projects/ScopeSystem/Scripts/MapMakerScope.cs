using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.FloorTimer.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using Carry.CarrySystem.Player.Scripts.Local;
using Carry.CarrySystem.RoutingAlgorithm.Interfaces;
using Carry.CarrySystem.SearchRoute.Scripts;
using Carry.CarrySystem.Spawners.Interfaces;
using Carry.CarrySystem.Spawners.Scripts;
using Carry.EditMapSystem.EditMap.Scripts;
using Carry.EditMapSystem.EditMapForPlayer.Scripts;
using Carry.UISystem.UI.MapMaker;
using Fusion;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using Projects.CarrySystem.Cart.Interfaces;
using Projects.CarrySystem.Item.Scripts;
using Projects.CarrySystem.Player.Scripts.Local;
using Projects.CarrySystem.SearchRoute.Scripts;
using Projects.MapMakerSystem.Scripts;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Carry.ScopeSystem.Scripts
{
    public sealed class MapMakerScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // asmdefが分割されているため、Mapの生成や編集はLocalEditScopeで行う
            // もし、LocalEditMapを変更したくなった場合は新しいクラスを作るべき
            // なぜなら、LocalEditMapはには単体で動くシーンが用意されていて、動作確認済みであるため
            
            // todo : とりあえず、playerをスポーンさせる
            
            // PrefabLoader 
            builder.Register<PrefabLoaderFromAddressable<CarryPlayerControllerLocal>>(Lifetime.Scoped)
                .As<IPrefabLoader<CarryPlayerControllerLocal>>()
                .WithParameter("path", "Prefabs/Players/CarryPlayerControllerLocal");
            
            builder.Register<PrefabLoaderFromAddressable<CartControllerNet>>(Lifetime.Scoped)
                .As<IPrefabLoader<CartControllerNet>>()
                .WithParameter("path", "Prefabs/Carts/CartControllerNet");
            
            
            // Player
            builder.Register<LocalCarryPlayerFactory>(Lifetime.Scoped).As<ICarryPlayerFactory>();
            builder.Register<CarryPlayerControllerLocalBuilder>(Lifetime.Scoped);
            builder.Register<LocalPlayerSpawner>(Lifetime.Scoped);
            builder.Register<CarryPlayerContainer>(Lifetime.Scoped);
            
            // Initializer
            builder.RegisterComponentInHierarchy<MapMakerInitializer>();


            // PrefabLoader 
            builder.Register<PrefabLoaderFromAddressable<CarryPlayerControllerNet>>(Lifetime.Scoped)
                .As<IPrefabLoader<CarryPlayerControllerNet>>()
                .WithParameter("path", "Prefabs/Players/CarryPlayerControllerNet");
            
            builder.Register<PrefabLoaderFromAddressable<CartControllerNet>>(Lifetime.Scoped)
                .As<IPrefabLoader<CartControllerNet>>()
                .WithParameter("path", "Prefabs/Carts/CartControllerNet");
            
            // Cart
            // todo : ここら辺のスクリプトはモックに切り替えられるかどうか考える
            builder.Register<HoldingBlockObserver>(Lifetime.Scoped);
            builder.Register<ReachRightEdgeChecker>(Lifetime.Scoped);
            builder.Register<RoutePresenterLocalSpawner>(Lifetime.Scoped).As<IRoutePresenterSpawner>();
            builder.Register<SearchAccessibleAreaPresenterBuilder>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<HoldingBlockNotifierLogger>().As<IHoldingBlockNotifier>();




            // Map
            // JsonとEntityGridMapに関する処理
            builder.Register<EntityGridMapBuilderLeaf>(Lifetime.Scoped).As<IEntityGridMapBuilder>();
            builder.Register<EntityGridMapDataConverter>(Lifetime.Scoped);
            builder.Register<EntityGridMapLoader>(Lifetime.Scoped);
            builder.Register<EntityGridMapSaver>(Lifetime.Scoped);
            builder.Register<StageMapSaver>(Lifetime.Scoped);
            builder.Register<MapValidator>(Lifetime.Scoped);
            builder.Register<MapTestPlayStarter>(Lifetime.Scoped);
            
            // 対応するプレハブをEntityGridMapを元に生成する
            builder.Register<LocalPlaceablePresenterSpawner>(Lifetime.Scoped).As<IPlaceablePresenterSpawner>();
            builder.Register<PlaceablePresenterBuilder>(Lifetime.Scoped);
            builder.Register<RandomWallPresenterPlacerLocal>(Lifetime.Scoped);
            builder.Register<RegularGroundPresenterPlacerLocal>(Lifetime.Scoped);
            builder.Register<PlaceablePresenterPlacer>(Lifetime.Scoped);
            builder.Register<LocalEditMapPresenterPlacerComposite>(Lifetime.Scoped).As<IPresenterPlacer>();

            // IMapUpdater
            // builder.Register<LocalCarryMapSwitcher>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            builder.Register<StageMapSwitcher>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            
            // Input
            builder.Register<EditMapBlockAttacher>(Lifetime.Scoped).As<IEditMapBlockAttacher>();
            builder.Register<MemorableEditMapBlockAttacher>(Lifetime.Scoped).WithParameter("capacity", 100);
            builder.Register<CUIHandleNumber>(Lifetime.Scoped);
            builder.Register<AutoSaveManager>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<MapMakerInput>();
            // builder.RegisterComponentInHierarchy<EditMapCUISave>();
            // builder.RegisterComponentInHierarchy<EditMapCUILoad>();

            // Presenter
            builder.RegisterComponentInHierarchy<LoadedFilePresenter>();
            
            // MapKey
            builder.RegisterComponentInHierarchy<MapKeyContainer>();

            // FloorTimer
            builder.RegisterComponentInHierarchy<PlayingCanvasUILocal>();
            builder.RegisterComponentInHierarchy<FloorTimerLocal>();
            builder.RegisterComponentInHierarchy<MapClearChecker>();
            builder.RegisterComponentInHierarchy<MapMakerUIManager>();
        }
    }
}