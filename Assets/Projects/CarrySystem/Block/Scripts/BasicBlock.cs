#nullable enable
using System;
using UnityEngine;

namespace Projects.CarrySystem.Block.Scripts
{
    // JSONファイルに書き出すためにSerializableをつける
    [Serializable]
    public class BasicBlockRecord
    {
        public BasicBlock.Kind kind;
    }
    public class BasicBlock :Block
    {
        public override Vector2Int GridPosition { get; set; }
        public BasicBlockRecord Record { get; }

        public enum Kind
        {
            None,
            Kind1,
        }

        public BasicBlock(BasicBlockRecord record,Vector2Int gridPosition)
        {
            Record = record;
            GridPosition = gridPosition;
        }

    }
}