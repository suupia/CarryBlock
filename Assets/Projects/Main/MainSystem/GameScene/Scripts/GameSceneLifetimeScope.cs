using VContainer;
using VContainer.Unity;
using UnityEngine;

namespace Main.VContainer
{
    public sealed class GameSceneLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ResourceAggregator>(Lifetime.Singleton);
            builder.Register<GameContext>(Lifetime.Singleton);
            builder.Register<WaveTimer>(Lifetime.Singleton);
            builder.Register<ReturnToMainBaseGauge>(Lifetime.Singleton);
        }
    }
}
