using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace  Carry.CarrySystem.CarryScene.Scripts
{
    public sealed class CarrySceneLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Serverのドメインスクリプト
            // Map
            builder.Register<EntityGridMapLoader>(Lifetime.Singleton);
            builder.Register<EntityGridMapSwitcher>(Lifetime.Singleton);
            builder.Register<TilePresenterRegister>(Lifetime.Singleton);
            
            // UI
            builder.Register<GameContext>(Lifetime.Singleton);
            builder.Register<FloorTimer>(Lifetime.Singleton);

            // Clientのドメインスクリプト
            // builder.Register<ReturnToMainBaseGauge>(Lifetime.Singleton);
        }
    }

}
