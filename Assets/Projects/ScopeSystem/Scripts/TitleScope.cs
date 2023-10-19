using Carry.GameSystem.TitleScene.Scripts;
using Carry.UISystem.UI.TitleScene;
using VContainer;
using VContainer.Unity;
#nullable enable

namespace Carry.ScopeSystem.Scripts
{
    public class TitleScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<OptionCanvasMono>();
            builder.RegisterComponentInHierarchy<TitleInitializer>();
        }
    }
}