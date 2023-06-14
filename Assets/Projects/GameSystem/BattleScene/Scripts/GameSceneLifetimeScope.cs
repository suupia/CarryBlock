using VContainer;
using VContainer.Unity;
using Nuts.BattleSystem.GameScene.Scripts;
using Nuts.Utility.Scripts;
using Nuts.BattleSystem.Enemy.Scripts;
using Nuts.BattleSystem.Player.Scripts;

namespace Nuts.BattleSystem.GameScene.Scripts
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