using System;
using Carry.CarrySystem.Block.Scripts;
using Projects.CarrySystem.Item.Interfaces;
using UnityEngine;
#nullable enable
namespace Projects.CarrySystem.Item.Scripts
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

        readonly TreasureCoinCounter _counter;
        public enum Kind
        {
            None,
            Kind1,
        }

        public TreasureCoin(TreasureCoin.Kind kind, Vector2Int gridPosition, TreasureCoinCounter counter)
        {
            KindValue = kind;
            GridPosition = gridPosition;
            _counter = counter;
        }
        
        public void OnGained()
        {
            _counter.Add(1);
            Debug.Log($"_counter.Count = {_counter.Count}");
        }

    }
}