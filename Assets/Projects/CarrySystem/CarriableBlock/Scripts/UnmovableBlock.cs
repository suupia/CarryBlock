using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.CarriableBlock.Interfaces;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using UnityEngine;
#nullable  enable

namespace Carry.CarrySystem.CarriableBlock.Scripts
{
    [Serializable]
    public record RockRecord
    {
        public UnmovableBlock.Kind[] kinds =new UnmovableBlock.Kind[5];
    }
    
    public class UnmovableBlock : ICarriableBlock
    {
        public Vector2Int GridPosition { get; set; }
        public　int MaxPlacedBlockCount { get; } = 1;
        public bool BeingCarried { get; set; } = false;
        public Kind KindValue { get; }

        public enum Kind
        {
            None,
            Kind1,
        }

        public UnmovableBlock(Kind kind,Vector2Int gridPosition)
        {
            KindValue = kind;
            GridPosition = gridPosition;
        }
        
        public bool CanPickUp()
        {
            return false;  // 常に持ち上げられない
        }

        public void  PickUp(IMoveExecutorSwitcher moveExecutorSwitcher, PlayerHoldingObjectContainer blockContainer, IHoldActionExecutor holdActionExecutor)
        {
            // 特になし
        }

        public bool CanPutDown(IList<ICarriableBlock> placedBlocks)
        {
            return false; // 常に置けない
        }
        
        public void PutDown(IMoveExecutorSwitcher moveExecutorSwitcher)
        {
            // 特になし
        }
    }
    
}
