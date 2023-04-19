using VContainer;
using VContainer.Unity;
using UnityEditorInternal;
using UnityEngine;

namespace Main.VContainer
{
    public sealed class GameSceneLifetimeScope : LifetimeScope
    {
        [SerializeField] WaveTimer _waveTimer;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ResourceAggregator>(Lifetime.Singleton);
            builder.Register<GameContext>(Lifetime.Singleton);
            builder.RegisterComponent(_waveTimer);
        }
    }
}
