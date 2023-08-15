using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
#nullable  enable

namespace Carry.CarrySystem.Block.Scripts
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

        public void  PickUp(ICharacter character)
        {
            // 特になし
        }

        public bool CanPutDown(IList<IBlock> blocks)
        {
            return false; // 常に置けない
        }
        
        public void PutDown(ICharacter character)
        {
            // 特になし
        }
    }
    
}