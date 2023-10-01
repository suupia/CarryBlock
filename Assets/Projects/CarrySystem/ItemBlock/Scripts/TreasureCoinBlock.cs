using System;
using Carry.CarrySystem.Block.Scripts;
using Projects.CarrySystem.ItemBlock.Interfaces;
using UnityEngine;

namespace Projects.CarrySystem.ItemBlock.Scripts
{
    [Serializable]
    public record TreasureCoinRecord
    {
        public TreasureCoinBlock.Kind[] kinds =new TreasureCoinBlock.Kind[5];
    }
    
    public class TreasureCoinBlock : IItemBlock
    {
        public Vector2Int GridPosition { get; set; }
        public TreasureCoinBlock.Kind KindValue { get; }


        public enum Kind
        {
            None,
            Kind1,
        }

        public TreasureCoinBlock(TreasureCoinBlock.Kind kind, Vector2Int gridPosition)
        {
            KindValue = kind;
            GridPosition = gridPosition;
        }

    }
}