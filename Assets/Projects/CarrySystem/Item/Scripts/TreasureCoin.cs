using System;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Map.Scripts;
using Projects.CarrySystem.Item.Interfaces;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

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
        public int MaxPlacedBlockCount { get; } = 1;
        public TreasureCoin.Kind KindValue { get; }

        readonly TreasureCoinCounter _counter;
        readonly EntityGridMap _map;
        public enum Kind
        {
            None,
            Kind1,
        }

        public TreasureCoin(TreasureCoin.Kind kind, Vector2Int gridPosition,EntityGridMap map, TreasureCoinCounter counter)
        {
            KindValue = kind;
            GridPosition = gridPosition;
            _map = map;
            _counter = counter;
        }
        
        public void OnGained(ItemControllerNet controller)
        {
            _counter.Add(1);
            _map.RemoveEntity(GridPosition,this);
        }

    }
}