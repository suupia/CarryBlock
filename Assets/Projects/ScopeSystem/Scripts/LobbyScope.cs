using System.Runtime.InteropServices.ComTypes;
using Carry.CarrySystem.CarryScene.Scripts;
using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using Carry.CarrySystem.Spawners.Scripts;
using Carry.UISystem.UI.LobbyScene;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using Fusion;
using Carry.GameSystem.Spawners.Scripts;
using Projects.CarrySystem.Item.Scripts;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Carry.GameSystem.LobbyScene.Scripts
{
    public sealed class LobbyScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // このシーンに遷移した時点でNetworkRunnerは存在していると仮定している
            var runner = FindObjectOfType<NetworkRunner>();
            Debug.Log($"NetworkRunner : {runner}");
            builder.RegisterComponent(runner);
            
            // PrefabLoader 
            builder.Register<PrefabLoaderFromAddressable<LobbyPlayerControllerNet>>(Lifetime.Scoped)
                .As<IPrefabLoader<LobbyPlayerControllerNet>>()
                .WithParameter("path", "Prefabs/Players/LobbyPlayerControllerNet");
            
            
            // NetworkRunnerに依存するスクリプト

            // Player
            builder.Register<MainLobbyPlayerFactory>(Lifetime.Scoped).As<ICarryPlayerFactory>();
            builder.Register<LobbyPlayerControllerNetBuilder>(Lifetime.Scoped).As<IPlayerControllerNetBuilder>();
            builder.Register<PlayerSpawner>(Lifetime.Scoped);
            builder.Register<LobbyPlayerContainer>(Lifetime.Scoped);

            // Map
            // JsonとEntityGridMapに関する処理
            builder.Register<EntityGridMapBuilderLeaf>(Lifetime.Scoped);
            builder.Register<EntityGridMapLoader>(Lifetime.Scoped);
            
            // 対応するプレハブをEntityGridMapを元に生成する
            builder.Register<LobbyWallPresenterPlacer>(Lifetime.Scoped);
            builder.Register<LobbyGroundPresenterPlacer>(Lifetime.Scoped);
            builder.Register<LobbyPresenterPlacerContainer>(Lifetime.Scoped).As<IPresenterPlacer>();
            builder.Register<PrefabLoaderFromAddressable<CartControllerNet>>(Lifetime.Scoped)
                .As<IPrefabLoader<CartControllerNet>>()
                .WithParameter("path", "Prefabs/Carts/CartLobbyControllerNet");
            
            builder.RegisterComponentInHierarchy<MapKeyDataSelectorNet>();
            
            //Item
            builder.Register<TreasureCoinCounter>(Lifetime.Scoped);
            
            // IMapUpdater
            builder.Register<LobbyMapUpdater>(Lifetime.Scoped).As<IMapUpdater>();
            
            // UI
            builder.RegisterComponentInHierarchy<SelectStageCanvasUINet>();

            builder.Register<LobbyStartGameTheater>(Lifetime.Scoped);
            
            builder.RegisterComponentInHierarchy<LobbyInitializer>();
            
        }
    }
}