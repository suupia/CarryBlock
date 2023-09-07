using System.Collections.Generic;
using Carry.CarrySystem.Map.Scripts;
using Projects.CarrySystem.Block.Scripts;

namespace Carry.CarrySystem.Map.Interfaces
{
    public interface IBlockBuilder
    {
        public (IReadOnlyList<BlockControllerNet>, IReadOnlyList<BlockPresenterNet>) Build(ref EntityGridMap map);
    }
}