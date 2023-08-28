using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;

namespace Projects.CarrySystem.Block.Scripts
{
    public class BlockMonoDelegateFactory
    {
        public IBlockMonoDelegate Create()
        {
            var basicBlock = default(BasicBlock);
            return new BlockMonoDelegate(basicBlock);
        }
    }
}