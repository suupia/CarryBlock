#nullable enable
using System;
using Projects.CarrySystem.Block.Interfaces;
using UnityEngine;

namespace Projects.CarrySystem.Block.Scripts
{
    // JSONファイルに書き出すためにSerializableをつける
    [Serializable]
    public record BasicBlockRecord
    {
        public BasicBlock.Kind[] kinds = new BasicBlock.Kind[10];
    }

    public class BasicBlock : IBlock
    {
        public Vector2Int GridPosition { get; set; }
        public int MaxPlacedBlockCount { get; } = 2;
        public bool BeingCarried { get; set; }
        public BasicBlock.Kind KindValue { get; }

        public enum Kind
        {
            None,
            Kind1,
        }

        public BasicBlock(BasicBlock.Kind kind, Vector2Int gridPosition)
        {
            KindValue = kind;
            GridPosition = gridPosition;
        }
    }
}