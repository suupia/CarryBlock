#nullable enable
using System;
using Projects.CarrySystem.Block.Interfaces;
using UnityEngine;

namespace Projects.CarrySystem.Block.Scripts
{
    // JSONファイルに書き出すためにSerializableをつける
    [Serializable]
    public class BasicBlockRecord
    {
        public BasicBlock.Kind kind;
    }

    public class BasicBlock : IBlock
    {
        public Vector2Int GridPosition { get; set; }
        public bool BeingCarried { get; set; }
        public BasicBlockRecord Record { get; }

        public enum Kind
        {
            None,
            Kind1,
        }

        public BasicBlock(BasicBlockRecord record, Vector2Int gridPosition)
        {
            Record = record;
            GridPosition = gridPosition;
        }
    }
}