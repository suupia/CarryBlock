using Carry.CarrySystem.Block.Interfaces;

namespace Projects.CarrySystem.Block.Scripts
{
    public class BlockMonoDelegateFactory
    {
        public IBlockMonoDelegate Create()
        {
            return new BlockMonoDelegate();
        }
    }
}