using System;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Gimmick.Interfaces;
using Carry.CarrySystem.Gimmick.Scripts;
using Carry.CarrySystem.Map.Scripts;
using Fusion;
using UniRx;
using UnityEngine;

namespace Projects.CarrySystem.Gimmick.Scripts
{
    [Serializable]
    public record SpikeGimmickRecord
    {
        public Spike.Kind[] kinds = new Spike.Kind[10];
    }
    
    public class Spike :  IGimmick
    {
        public Vector2Int GridPosition { get; set; }
        public int MaxPlacedBlockCount { get; } = 1;
        public Spike.Kind KindValue { get; }
        
        public enum Kind
        {
            None,
            Kind1,
        }
        
        public Spike(Kind kind, Vector2Int gridPosition)
        {
            KindValue = kind;
            GridPosition = gridPosition;
        }
        
        public void StartGimmick(NetworkRunner runner)
        {
            Debug.Log("StartGimmick GridPosition:" + GridPosition + " Kind:" + KindValue);
        }
        
        public void EndGimmick(NetworkRunner runner)
        {
            Debug.Log("EndGimmick GridPosition:" + GridPosition + " Kind:" + KindValue);
        }
    }
}