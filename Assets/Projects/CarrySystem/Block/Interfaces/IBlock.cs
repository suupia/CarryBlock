using System.Collections.Generic;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Block.Interfaces
{
    public interface IBlock : IEntity
    {
        int MaxPlacedBlockCount { get; }
        bool CanPickUp();
        void PickUp(ICharacter character);
        bool CanPutDown(IList<IBlock> blocks);
        void PutDown(ICharacter character);
    }

}