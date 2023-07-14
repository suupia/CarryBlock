using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using Carry.CarrySystem.Spawners;
using Fusion;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Nuts.NetworkUtility.NetworkRunnerManager.Scripts;
using Nuts.Utility.Scripts;

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class EditMapLifetimeScope: LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Map
            builder.Register<EntityGridMapLoader>(Lifetime.Scoped);
            builder.Register<TilePresenterBuilder>(Lifetime.Scoped);
            builder.Register<EntityGridMapSwitcher>(Lifetime.Scoped);
            builder.Register<TilePresenterAttacher>(Lifetime.Scoped);
            
            // Initializer
            builder.Register<EditMapInitializer>(Lifetime.Scoped);
        }

    }
}