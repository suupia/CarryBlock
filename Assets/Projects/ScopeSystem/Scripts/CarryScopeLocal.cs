#nullable enable
using Carry.CarrySystem.CarryScene.Scripts;
using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.FloorTimer.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using Carry.CarrySystem.RoutingAlgorithm.Interfaces;
using Carry.CarrySystem.SearchRoute.Scripts;
using Carry.CarrySystem.Spawners.Interfaces;
using Carry.CarrySystem.Spawners.Scripts;
using Carry.UISystem.UI.CarryScene;
using Fusion;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using Projects.CarrySystem.Cart.Interfaces;
using Projects.CarrySystem.Gimmick.Scripts;
using Projects.CarrySystem.Item.Scripts;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Carry.ScopeSystem.Scripts
{
    public sealed class CarryScopeLocal : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // MapKeyDataSelectorNet is DontDestroyOnLoad Object
            var mapKeyDataSelectorLocal = FindObjectOfType<MapKeyDataSelectorLocal>();
            Debug.Log($"MapKeyDataSelectorNet : {mapKeyDataSelectorLocal}");
            builder.RegisterComponent(mapKeyDataSelectorLocal);

            // PrefabLoader 
            builder.Register<PrefabLoaderFromAddressable<CarryPlayerControllerLocal>>(Lifetime.Scoped)
                .As<IPrefabLoader<CarryPlayerControllerLocal>>()
                .WithParameter("path", "Prefabs/Players/CarryPlayerControllerLocal");

            builder.Register<PrefabLoaderFromAddressable<CartControllerLocal>>(Lifetime.Scoped)
                .As<IPrefabLoader<CartControllerLocal>>()
                .WithParameter("path", "Prefabs/Carts/CartControllerLocal");

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
            builder.Register<NetworkPlaceablePresenterSpawner>(Lifetime.Scoped).As<IPlaceablePresenterSpawner>();
            builder.Register<PlaceablePresenterBuilder>(Lifetime.Scoped);
            builder.Register<PlaceablePresenterPlacer>(Lifetime.Scoped);
            builder.Register<LocalGroundPresenterPlacer>(Lifetime.Scoped);
            builder.Register<LocalWallPresenterPlacer>(Lifetime.Scoped);
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
            
            // Gimmick
            builder.RegisterComponentInHierarchy<GimmickDisposerMono>();

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