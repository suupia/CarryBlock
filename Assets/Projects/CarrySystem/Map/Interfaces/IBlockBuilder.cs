using System.Collections.Generic;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Map.Scripts;

namespace Carry.CarrySystem.Map.Interfaces
{
    public interface IBlockBuilder
    {
        public (IReadOnlyList<BlockControllerNet>, IReadOnlyList<BlockPresenterNet>) Build(ref EntityGridMap map);
    }
}