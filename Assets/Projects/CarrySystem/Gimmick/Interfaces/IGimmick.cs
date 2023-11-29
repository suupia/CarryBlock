using System;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Entity.Interfaces;
using Fusion;

# nullable enable

namespace Carry.CarrySystem.Gimmick.Interfaces
{
    public interface IGimmick : IPlaceable , IDisposable
    {
        public void StartGimmick();
    }
}