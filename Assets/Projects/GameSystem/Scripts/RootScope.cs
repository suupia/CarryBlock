using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Audio.Scripts;
using Carry.CarrySystem.CarryScene.Scripts;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using Carry.CarrySystem.Spawners.Scripts;
using Fusion;
using Carry.GameSystem.LobbyScene.Scripts;
using Carry.GameSystem.Scripts;
using Carry.Utility.Scripts;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace  Carry.GameSystem.Scripts
{
    public class RootScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<PlayerCharacterTransporter>(Lifetime.Singleton);
            builder.Register<StageIndexTransporter>(Lifetime.Singleton);
            builder.Register<CarryInitializersReady>(Lifetime.Singleton);
            builder.Register<OptionSettingsTransporter>(Lifetime.Singleton);
            builder.Register<EditingMapTransporter>(Lifetime.Singleton);
        }
    }

}
