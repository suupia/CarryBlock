using VContainer;
using VContainer.Unity;
using Nuts.Projects.GameSystem.GameScene.Scripts;
using Main;
using Nuts.BattleSystem.Enemy.Scripts;
using Nuts.BattleSystem.Enemy.Scripts.Player.Scripts;

namespace Nuts.Projects.GameSystem.GameScene.Scripts
{
    public sealed class GameSceneLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Serverのドメインスクリプト
            builder.Register<ResourceAggregator>(Lifetime.Singleton);
            builder.Register<GameContext>(Lifetime.Singleton);
            builder.Register<WaveTimer>(Lifetime.Singleton);
            // Localのドメインスクリプト
            builder.Register<ReturnToMainBaseGauge>(Lifetime.Singleton);
        }
    }
}