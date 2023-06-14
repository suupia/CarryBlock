using Nuts.BattleSystem.Enemy.Scripts;
using Nuts.BattleSystem.GameScene.Scripts;
using VContainer;
using VContainer.Unity;

namespace Nuts.BattleSystem.LobbyScene.Scripts
{
    public sealed class LobbySceneLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ResourceAggregator>(Lifetime.Singleton);
            builder.Register<GameContext>(Lifetime.Singleton);
            builder.Register<WaveTimer>(Lifetime.Singleton);
        }
    }
}