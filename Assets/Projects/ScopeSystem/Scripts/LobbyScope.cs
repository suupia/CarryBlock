using System.Runtime.InteropServices.ComTypes;
using Carry.CarrySystem.CarryScene.Scripts;
using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using Carry.CarrySystem.Spawners;
using Carry.UISystem.UI.LobbyScene;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using Fusion;
using Carry.GameSystem.Spawners.Scripts;
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
            builder.Register<LobbyPlayerBuilder>(Lifetime.Scoped).As<IPlayerBuilder>();
            builder.Register<PlayerSpawner>(Lifetime.Scoped);
            
            // Map
            // JsonとEntityGridMapに関する処理
            builder.Register<EntityGridMapBuilder>(Lifetime.Scoped).As<IEntityGridMapBuilder>();
            builder.Register<EntityGridMapLoader>(Lifetime.Scoped);
            
            // 対応するプレハブをEntityGridMapを元に生成する
            builder.Register<WallPresenterPlacer>(Lifetime.Scoped);
            builder.Register<GroundPresenterPlacer>(Lifetime.Scoped);
            builder.Register<LobbyPresenterPlacerContainer>(Lifetime.Scoped).As<IPresenterPlacer>();
            
            // IMapUpdater
            builder.Register<LobbyMapUpdater>(Lifetime.Scoped).As<IMapUpdater>();
            
            // UI
            builder.RegisterComponentInHierarchy<SelectStageCanvasUINet>();
            
            builder.RegisterComponentInHierarchy<LobbyInitializer>();
            
        }
    }
}