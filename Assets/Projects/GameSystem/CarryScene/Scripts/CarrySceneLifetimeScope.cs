using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
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
            builder.Register<EntityGridMapGenerator>(Lifetime.Singleton);
            builder.Register<EntityGridMapSwitcher>(Lifetime.Singleton);
            builder.Register<TilePresenterRegister>(Lifetime.Singleton);
            
            // Player
            builder.Register<ICharacterMove,QuickTurnMove>(Lifetime.Singleton);
            builder.Register<ICharacterAction,CharacterAction>(Lifetime.Singleton);
            builder.Register<Character>(Lifetime.Singleton);

            // Clientのドメインスクリプト
            // builder.Register<ReturnToMainBaseGauge>(Lifetime.Singleton);
        }
    }

}
