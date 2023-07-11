using Carry.CarrySystem.CarryScene.Scripts;
using Carry.CarrySystem.Player.Scripts;
using Carry.CarrySystem.Spawners;
using Fusion;
using Nuts.BattleSystem.Enemy.Scripts;
using Nuts.BattleSystem.GameScene.Scripts;
using Nuts.BattleSystem.Player.Scripts;
using Nuts.BattleSystem.Spawners.Scripts;
using Nuts.Utility.Scripts;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using GameContext = Nuts.BattleSystem.GameScene.Scripts.GameContext;

namespace Nuts.BattleSystem.LobbyScene.Scripts
{
    public sealed class LobbySceneLifetimeScope : LifetimeScope
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
            builder.Register<GameContext>(Lifetime.Singleton);
            builder.Register<WaveTimer>(Lifetime.Singleton);
        }
    }
}