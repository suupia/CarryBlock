using Carry.CarrySystem.CarryScene.Scripts;
using Carry.CarrySystem.Player.Scripts;
using Carry.CarrySystem.Spawners;
using Fusion;
using Projects.BattleSystem.Enemy.Scripts;
using Projects.BattleSystem.Player.Scripts;
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
            var runner = FindObjectOfType<NetworkRunner>();
            builder.RegisterComponent(runner);
            
            // NetworkRunnerに依存するスクリプト
            builder.Register<LobbyNetworkPlayerPrefabSpawner>(Lifetime.Scoped).As< IPrefabSpawner<LobbyNetworkPlayerController>>();
            builder.Register<LobbyNetworkPlayerSpawner>(Lifetime.Scoped);
            builder.Register<LobbyNetworkPlayerContainer>(Lifetime.Scoped);

            builder.Register<EnemySpawner>(Lifetime.Scoped);
            builder.Register<NetworkEnemyContainer>(Lifetime.Scoped);
            

            builder.RegisterComponentInHierarchy<LobbyInitializer>();
            
            builder.Register<ResourceAggregator>(Lifetime.Singleton);
        }
    }
}