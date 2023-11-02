#nullable enable

using System.Collections.Generic;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;

namespace Carry.CarrySystem.CarriableBlock.Interfaces
{
    public interface ICarriableBlock : IBlock
    {
        bool CanPickUp();
        void  PickUp(IMoveExecutorSwitcher moveExecutorSwitcher, PlayerHoldingObjectContainer blockContainer, IHoldActionExecutor holdActionExecutor);
        bool CanPutDown(IList<ICarriableBlock> placedBlocks);
        void PutDown(IMoveExecutorSwitcher moveExecutorSwitcher);
    }
}