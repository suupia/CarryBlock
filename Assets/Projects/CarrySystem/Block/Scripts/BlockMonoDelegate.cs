using Carry.CarrySystem.Block.Interfaces;
using Projects.CarrySystem.Block.Info;

namespace Projects.CarrySystem.Block.Scripts
{
    public class BlockMonoDelegate : IBlockMonoDelegate
    {
         public BlockInfo Info { get; private set; }

         public BlockMonoDelegate()
         {
             
         }
         
         public void SetInfo(BlockInfo info)
         {
             Info = info;
         }
    }
}