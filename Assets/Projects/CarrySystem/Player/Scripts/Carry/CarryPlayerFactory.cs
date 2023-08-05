﻿using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
using VContainer;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    // ToDo: このクラス使っていないならいらない？
    // 何かPlayerのドメインを差し替えたいときに使っていたのかも
    public class CarryPlayerFactory : ICarryPlayerFactory
    {
        readonly IMapUpdater _mapUpdater;
        [Inject]
        public CarryPlayerFactory(IMapUpdater mapUpdater)
        {
            _mapUpdater = mapUpdater;
        }
        public ICharacter Create(PlayerColorType colorType)
        {
            // ToDo: switch文で分ける
            var moveExe = new QuickTurnMoveExecutor();
            var holdExe = new HoldActionExecutor(_mapUpdater);
            var passExe = new PassActionExecutor(holdExe,10, LayerMask.GetMask("Player"));
            var character = new Character(moveExe, holdExe, passExe);
            return character;
        }
    }
}