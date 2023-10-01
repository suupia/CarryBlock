using System;
using Carry.CarrySystem.Block.Scripts;
using Projects.CarrySystem.ItemBlock.Interfaces;
using UnityEngine;

namespace Projects.CarrySystem.ItemBlock.Scripts
{
    [Serializable]
    public record TreasureCoinRecord
    {
        public TreasureCoin.Kind[] kinds =new TreasureCoin.Kind[5];
    }
    
    public class TreasureCoin : IItem
    {
        public Vector2Int GridPosition { get; set; }
        public TreasureCoin.Kind KindValue { get; }


        public enum Kind
        {
            None,
            Kind1,
        }

        public TreasureCoin(TreasureCoin.Kind kind, Vector2Int gridPosition)
        {
            KindValue = kind;
            GridPosition = gridPosition;
        }

    }
}