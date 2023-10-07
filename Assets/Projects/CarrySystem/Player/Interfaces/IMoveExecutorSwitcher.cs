﻿using Carry.CarrySystem.Player.Info;
using UnityEngine;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IMoveExecutorSwitcher : IMoveExecutor
    {
        public void SwitchToBeforeMoveExecutor();
        public void SwitchToRegularMove();
        public void SwitchToDashMove();
        public void SwitchToSlowMove();
        public void SwitchToConfusionMove();
        public void SwitchToFaintedMove();
    }
}