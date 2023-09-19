using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using Carry.CarrySystem.Spawners;
using Fusion;
using Projects.BattleSystem.LobbyScene.Scripts;
using Projects.BattleSystem.Scripts;
using Projects.Utility.Scripts;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace  Carry.GameSystem.Scripts
{
    public class RootScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<PlayerCharacterHolder>(Lifetime.Singleton);
            builder.Register<StageIndexTransporter>(Lifetime.Singleton);

        }
    }

}
