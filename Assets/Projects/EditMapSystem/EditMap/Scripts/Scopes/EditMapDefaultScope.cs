using Carry.CarrySystem.Map.Scripts;
using Fusion;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class EditMapDefaultScope: LifetimeScope
    {
        // Defaultマップのみを作成するシーンのスコープ
        protected override void Configure(IContainerBuilder builder)
        {
            // Map
            builder.Register<EntityGridMapSaver>(Lifetime.Scoped);

            // Initializer
            builder.RegisterComponentInHierarchy<EditMapDefaultInitializer>();
        }
    }
}