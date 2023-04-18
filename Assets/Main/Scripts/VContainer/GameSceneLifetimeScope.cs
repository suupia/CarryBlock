using VContainer;
using VContainer.Unity;
using UnityEditorInternal;

namespace Main.VContainer
{
    public sealed class GameSceneLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ResourceAggregator>(Lifetime.Singleton);
        }
    }
}
