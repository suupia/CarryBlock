﻿using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Entity.Interfaces;
using Fusion;

# nullable enable

namespace Carry.CarrySystem.Gimmick.Interfaces
{
    public interface IGimmick : IPlaceable
    {
        public void StartGimmick(NetworkRunner runner);
        public void EndGimmick(NetworkRunner runner);
    }
}