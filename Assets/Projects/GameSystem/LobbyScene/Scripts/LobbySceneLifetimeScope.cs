using VContainer;
using VContainer.Unity;
using Enemy;
using Nuts.Projects.GameSystem.GameScene.Scripts;

namespace Main.VContainer
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