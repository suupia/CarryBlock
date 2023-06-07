using Nuts.BattleSystem.Enemy.Scripts;
using VContainer;
using VContainer.Unity;
using Nuts.GameSystem.GameScene.Scripts;

namespace Nuts.GameSystem.LobbyScene.Scripts
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