using VContainer;
using VContainer.Unity;

namespace Main.VContainer
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