using System.Collections.Generic;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using Projects.CarrySystem.Block.Info;
using UnityEngine;

namespace Projects.CarrySystem.Block.Scripts
{
    public class BlockMonoDelegate : IBlockMonoDelegate
    {
         public BlockInfo Info { get; private set; }
         public IBlock Block => _block;

         readonly IBlock _block;

         public BlockMonoDelegate(IBlock block)
         {
             _block = block;
         }

         public void SetInfo(BlockInfo info)
         {
             Info = info;
         }
         
         // IBlock implementation
         public Vector2Int GridPosition { get => _block.GridPosition; set => _block.GridPosition = value; }
         public int MaxPlacedBlockCount => _block.MaxPlacedBlockCount;
         public bool CanPickUp() => _block.CanPickUp();
         public void PickUp(ICharacter character) => _block.PickUp(character);
         public bool CanPutDown(IList<IBlock> blocks) => _block.CanPutDown(blocks);
         public void PutDown(ICharacter character) => _block.PutDown(character);
    }
}