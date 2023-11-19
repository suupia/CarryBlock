using System.Collections.Generic;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
#nullable enable

namespace Carry.CarrySystem.CarriableBlock.Interfaces
{
    public interface ICarriableBlock : IBlock
    {
        bool CanPickUp();
        void PickUp(IMoveExecutorSwitcher moveExecutorSwitcher, IHoldActionExecutor holdActionExecutor);
        bool CanPutDown(IList<ICarriableBlock> placedBlocks);
        void PutDown(IMoveExecutorSwitcher moveExecutorSwitcher);
    }
}