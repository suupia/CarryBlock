using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Scripts;
using Nuts.BattleSystem.Enemy.Scripts;
using Nuts.BattleSystem.GameScene.Scripts;
using Nuts.BattleSystem.Player.Scripts;
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
            builder.Register<EntityGridMapSwitcher>(Lifetime.Singleton);
            // Localのドメインスクリプト
            // builder.Register<ReturnToMainBaseGauge>(Lifetime.Singleton);
        }
    }

}
