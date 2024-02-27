#nullable enable
using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using Carry.CarrySystem.Spawners.Interfaces;
using Carry.CarrySystem.Spawners.Scripts;
using Carry.GameSystem.LobbyScene.Scripts;
using Carry.UISystem.UI.LobbyScene;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using Fusion;
using Projects.CarrySystem.Item.Scripts;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Projects.ScopeSystem.Scripts
{
    public sealed class LobbyScopeLocal : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // PrefabLoader 
            builder.Register<PrefabLoaderFromAddressable<LobbyPlayerControllerNet>>(Lifetime.Scoped)
                .As<IPrefabLoader<LobbyPlayerControllerNet>>()
                .WithParameter("path", "Prefabs/Players/LobbyPlayerControllerNet");
            

            // Player
            builder.Register<LobbyPlayerFactory>(Lifetime.Scoped).As<ICarryPlayerFactory>();
            builder.Register<LobbyPlayerControllerNetBuilder>(Lifetime.Scoped).As<IPlayerControllerNetBuilder>();
            builder.Register<NetworkPlayerSpawner>(Lifetime.Scoped);
            builder.Register<LobbyPlayerContainer>(Lifetime.Scoped);

            // Map
            // JsonとEntityGridMapに関する処理
            builder.Register<EntityGridMapBuilderLeaf>(Lifetime.Scoped).As<IEntityGridMapBuilder>();
            builder.Register<EntityGridMapLoader>(Lifetime.Scoped);
            
            // 対応するプレハブをEntityGridMapを元に生成する
            builder.Register<LobbyWallPresenterPlacer>(Lifetime.Scoped);
            builder.Register<GroundPresenterNetSpawner>(Lifetime.Scoped).As<IGroundPresenterSpawner>();
            builder.Register<GroundPresenterPlacer>(Lifetime.Scoped);
            builder.Register<LobbyPresenterPlacerComposite>(Lifetime.Scoped).As<IPresenterPlacer>();
            builder.Register<PrefabLoaderFromAddressable<CartControllerNet>>(Lifetime.Scoped)
                .As<IPrefabLoader<CartControllerNet>>()
                .WithParameter("path", "Prefabs/Carts/CartLobbyControllerNet");
            
            builder.RegisterComponentInHierarchy<MapKeyDataSelectorNet>();
            
            //Item
            builder.Register<TreasureCoinCounter>(Lifetime.Scoped);
            
            // IMapUpdater
            builder.Register<LobbyMapSwitcher>(Lifetime.Scoped).As<IMapSwitcher>();
            
            // UI
            builder.RegisterComponentInHierarchy<SelectStageCanvasUINet>();

            builder.Register<LobbyStartGameTheater>(Lifetime.Scoped);
            
            builder.RegisterComponentInHierarchy<LobbyInitializer>();
            
        }
    }
}