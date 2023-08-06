using Carry.CarrySystem.CarryScene.Scripts;
using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using Carry.CarrySystem.Spawners;
using Fusion;
using Projects.BattleSystem.Spawners.Scripts;
using Projects.Utility.Scripts;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Projects.BattleSystem.LobbyScene.Scripts
{
    public sealed class LobbyLifetimeScope : LifetimeScope
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
            builder.Register<MoveExecutorContainer>(Lifetime.Scoped);
            builder.Register<MainLobbyPlayerFactory>(Lifetime.Scoped).As<ICarryPlayerFactory>();
            builder.Register<LobbyPlayerBuilder>(Lifetime.Scoped).As<IPlayerBuilder>();
            builder.Register<PlayerSpawner>(Lifetime.Scoped);

            
            
            builder.RegisterComponentInHierarchy<LobbyInitializer>();
            
        }
    }
}