using System.Collections.Generic;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Block.Interfaces
{
    // If a block implementing this interface is placed, the cart route will be blocked by this block.
    public interface IBlock : IEntity
    {

    }

}