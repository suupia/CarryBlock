using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.FloorTimer.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using Carry.CarrySystem.SearchRoute.Scripts;
using Carry.CarrySystem.Spawners.Interfaces;
using Carry.CarrySystem.Spawners.Scripts;
using Fusion;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using Projects.CarrySystem.Item.Scripts;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Carry.ScopeSystem.Scripts
{
    public sealed class LocalCarryScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // todo : とりあえず、playerをスポーンさせる
            
            // PrefabLoader 
            builder.Register<PrefabLoaderFromAddressable<CarryPlayerControllerNet>>(Lifetime.Scoped)
                .As<IPrefabLoader<CarryPlayerControllerNet>>()
                .WithParameter("path", "Prefabs/Players/CarryPlayerControllerNet");
            
            builder.Register<PrefabLoaderFromAddressable<CartControllerNet>>(Lifetime.Scoped)
                .As<IPrefabLoader<CartControllerNet>>()
                .WithParameter("path", "Prefabs/Carts/CartControllerNet");
            
            
            // Player
            builder.Register<MainCarryPlayerFactory>(Lifetime.Scoped).As<ICarryPlayerFactory>();
            builder.Register<CarryPlayerControllerNetBuilder>(Lifetime.Scoped).As<IPlayerControllerNetBuilder>();
            builder.Register<PlayerSpawner>(Lifetime.Scoped);
            builder.Register<CarryPlayerContainer>(Lifetime.Scoped);
            
            // Initializer
            builder.RegisterComponentInHierarchy<LocalCarryInitializer>();


            // // PrefabLoader 
            // builder.Register<PrefabLoaderFromAddressable<CarryPlayerControllerNet>>(Lifetime.Scoped)
            //     .As<IPrefabLoader<CarryPlayerControllerNet>>()
            //     .WithParameter("path", "Prefabs/Players/CarryPlayerControllerNet");
            //
            // builder.Register<PrefabLoaderFromAddressable<CartControllerNet>>(Lifetime.Scoped)
            //     .As<IPrefabLoader<CartControllerNet>>()
            //     .WithParameter("path", "Prefabs/Carts/CartControllerNet");
            //
            //
            // // Player
            // builder.Register<MainCarryPlayerFactory>(Lifetime.Scoped).As<ICarryPlayerFactory>();
            // builder.Register<CarryPlayerBuilder>(Lifetime.Scoped).As<IPlayerBuilder>();
            // builder.Register<PlayerSpawner>(Lifetime.Scoped);
            // builder.Register<CarryPlayerContainer>(Lifetime.Scoped);
            //
            //
            // // Map
            // // JsonからEntityGridMapを生成する
            // builder.Register<EntityGridMapBuilder>(Lifetime.Scoped);
            // builder.Register<EntityGridMapLoader>(Lifetime.Scoped);
            //
            // // 対応するプレハブをEntityGridMapを元に生成する
            // builder.Register<CarryBlockBuilder>(Lifetime.Scoped);
            // builder.Register<CarryBlockPresenterPlacer>(Lifetime.Scoped);
            // builder.Register<RandomWallPresenterPlacerNet>(Lifetime.Scoped);
            // builder.Register<RegularGroundPresenterPlacerLocal>(Lifetime.Scoped);
            // builder.Register<LocalGroundPresenterPlacer>(Lifetime.Scoped);
            // builder.Register<LocalWallPresenterPlacer>(Lifetime.Scoped);
            // builder.Register<CarryBlockPresenterPlacer>(Lifetime.Scoped);
            // builder.Register<RandomWallPresenterPlacerNet>(Lifetime.Scoped);
            // builder.Register<RegularGroundPresenterPlacerLocal>(Lifetime.Scoped);
            // builder.RegisterComponentInHierarchy<PresenterPlacerNet>();
            //
            //
            // // IMapUpdater
            // builder.Register<EntityGridMapSwitcher>(Lifetime.Scoped).As<IMapUpdater>();
            //
            //
            // // Cart
            // builder.Register<CartBuilder>(Lifetime.Scoped);
            // builder.Register<CartShortestRouteMove>(Lifetime.Scoped);
            // builder.Register<SearchAccessibleAreaBuilder>(Lifetime.Scoped);
            // builder.Register<HoldingBlockObserver>(Lifetime.Scoped);
            // builder.Register<ReachRightEdgeChecker>(Lifetime.Scoped);
            //
            // //Item
            // builder.Register<TreasureCoinCounter>(Lifetime.Scoped);
            //
            // // UI
            // builder.RegisterComponentInHierarchy<FloorTimerNet>();
            //
            // // Notifier
            // builder.RegisterComponentInHierarchy<CartMovementNotifierNet>();
            //
            // // Handler
            // builder.Register<PlayerFollowMovingCart>(Lifetime.Scoped);
            // builder.RegisterComponentInHierarchy<PlayerNearCartHandlerNet>();
            
            
            // 以下はLocalEditMapScopeからコピー
            
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
            builder.Register<LocalCarryMapSwitcher>(Lifetime.Scoped).As<IMapUpdater>();
            
            // // Input
            // builder.Register<EditMapBlockAttacher>(Lifetime.Scoped);
            // builder.Register<CUIHandleNumber>(Lifetime.Scoped);
            // builder.Register<AutoSaveManager>(Lifetime.Scoped);
            // builder.RegisterComponentInHierarchy<EditMapInput>();
            // builder.RegisterComponentInHierarchy<EditMapCUISave>();
            // builder.RegisterComponentInHierarchy<EditMapCUILoad>();
            //
            //             
            // //Item
            // builder.Register<TreasureCoinCounter>(Lifetime.Scoped);
            //
            // // Presenter
            // builder.RegisterComponentInHierarchy<LoadedFilePresenter>();
            //
            // // MapKey
            // builder.RegisterComponentInHierarchy<MapKeyContainer>();
            //
            // // Initializer
            // builder.RegisterComponentInHierarchy<LocalEditMapInitializer>();

        }
    }
}