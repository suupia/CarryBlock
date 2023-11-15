using Carry.CarrySystem.CarryScene.Scripts;
using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.FloorTimer.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using Carry.CarrySystem.RoutingAlgorithm.Interfaces;
using Carry.CarrySystem.SearchRoute.Scripts;
using Carry.CarrySystem.Spawners.Scripts;
using Carry.UISystem.UI.CarryScene;
using Fusion;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using Projects.CarrySystem.Cart.Interfaces;
using Projects.CarrySystem.Item.Scripts;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Carry.ScopeSystem.Scripts
{
    public sealed class CarryScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // このシーンに遷移した時点でNetworkRunnerは存在していると仮定している
            var runner = FindObjectOfType<NetworkRunner>();
            Debug.Log($"NetworkRunner : {runner}");
            builder.RegisterComponent(runner);
            
            // MapKeyDataSelectorNet is DontDestroyOnLoad Object
            var mapKeyDataSelectorNet = FindObjectOfType<MapKeyDataSelectorNet>();
            Debug.Log($"MapKeyDataSelectorNet : {mapKeyDataSelectorNet}");
            builder.RegisterComponent(mapKeyDataSelectorNet);

            // PrefabLoader 
            builder.Register<PrefabLoaderFromAddressable<CarryPlayerControllerNet>>(Lifetime.Scoped)
                .As<IPrefabLoader<CarryPlayerControllerNet>>()
                .WithParameter("path", "Prefabs/Players/CarryPlayerControllerNet");

            builder.Register<PrefabLoaderFromAddressable<CartControllerNet>>(Lifetime.Scoped)
                .As<IPrefabLoader<CartControllerNet>>()
                .WithParameter("path", "Prefabs/Carts/CartControllerNet");

            // NetworkRunnerに依存するスクリプト

            // Player
            builder.Register<CarryPlayerFactory>(Lifetime.Scoped).As<ICarryPlayerFactory>();
            builder.Register<CarryPlayerControllerNetBuilder>(Lifetime.Scoped).As<IPlayerControllerNetBuilder>();
            builder.Register<NetworkPlayerSpawner>(Lifetime.Scoped);
            builder.Register<CarryPlayerContainer>(Lifetime.Scoped);


            // Map
            // JsonからEntityGridMapを生成する
            builder.Register<IEntityGridMapBuilder>(container =>
            {
                var treasureCoinCounter = container.Resolve<TreasureCoinCounter>();
                return new EntityGridMapBuilderWithTreasureCoin(new EntityGridMapBuilderLeaf(),treasureCoinCounter);
            }, Lifetime.Scoped);
            builder.Register<EntityGridMapLoader>(Lifetime.Scoped);
            
            // 対応するプレハブをEntityGridMapを元に生成する
            builder.Register<CarryBlockBuilder>(Lifetime.Scoped);
            builder.Register<CarryBlockPresenterPlacer>(Lifetime.Scoped);
            builder.Register<RandomWallPresenterPlacerNet>(Lifetime.Scoped);
            builder.Register<RegularGroundPresenterPlacerLocal>(Lifetime.Scoped);
            builder.Register<LocalGroundPresenterPlacer>(Lifetime.Scoped);
            builder.Register<LocalWallPresenterPlacer>(Lifetime.Scoped);
            builder.Register<CarryBlockPresenterPlacer>(Lifetime.Scoped);
            builder.Register<RandomWallPresenterPlacerNet>(Lifetime.Scoped);
            builder.Register<RegularGroundPresenterPlacerLocal>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<PresenterPlacerNet>();
            
            
            // IMapUpdater
            builder.Register<EntityGridMapSwitcher>(Lifetime.Scoped).As<IMapSwitcher>().As<IMapGetter>().AsSelf();


            // Cart
            builder.Register<CartBuilder>(Lifetime.Scoped);
            builder.Register<CartShortestRouteMove>(Lifetime.Scoped);
            builder.Register<RoutePresenterNetSpawner>(Lifetime.Scoped).As<IRoutePresenterSpawner>();
            builder.Register<SearchAccessibleAreaPresenterBuilder>(Lifetime.Scoped);
            builder.Register<HoldingBlockObserver>(Lifetime.Scoped);
            builder.Register<ReachRightEdgeChecker>(Lifetime.Scoped);
            
            // Item
            builder.Register<TreasureCoinCounter>(Lifetime.Scoped);

            // UI
            builder.RegisterComponentInHierarchy<FloorTimerNet>();
            builder.RegisterComponentInHierarchy<ResultCanvasUINet>();

            // Notifier
            builder.RegisterComponentInHierarchy<HoldingBlockNotifierNet>().As<IHoldingBlockNotifier>();

            // Handler
            builder.Register<PlayerFollowMovingCart>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<PlayerNearCartHandlerNet>();

            // Initializer
            builder.RegisterComponentInHierarchy<CarryInitializer>();

            // View
            builder.RegisterComponentInHierarchy<PlayingCanvasUINet>();
            
            // PostEffect
            builder.RegisterComponentInHierarchy<VignetteBlinker>();

            // Clientのドメインスクリプト
            // builder.Register<ReturnToMainBaseGauge>(Lifetime.Singleton);
        }
    }
}