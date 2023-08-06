using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Projects.CarrySystem.Block.Interfaces;
using UnityEngine;
#nullable  enable

namespace Carry.CarrySystem.Entity.Scripts
{
    [Serializable]
    public record RockRecord
    {
        public UnmovableBlock.Kind[] kinds =new UnmovableBlock.Kind[5];
    }
    
    public class UnmovableBlock : IBlock
    {
        public Vector2Int GridPosition { get; set; }
        public　int MaxPlacedBlockCount { get; } = 1;
        public bool BeingCarried { get; set; } = false;
        public UnmovableBlock.Kind KindValue { get; }

        public enum Kind
        {
            None,
            Kind1,
        }

        public UnmovableBlock(UnmovableBlock.Kind kind,Vector2Int gridPosition)
        {
            KindValue = kind;
            GridPosition = gridPosition;
        }
        
        public bool CanPickUp()
        {
            return false;  // 常に持ち上げられない
        }

        public void  PickUp()
        {
            // 特になし
        }

        public bool CanPutDown(IList<IBlock> blocks)
        {
            return false; // 常に置けない
        }
        
        public void PutDown()
        {
            // 特になし
        }
    }
    
}
