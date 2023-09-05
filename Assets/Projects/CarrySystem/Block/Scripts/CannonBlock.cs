﻿using System;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Block.Scripts
{
    // JSONファイルに書き出すためにSerializableをつける
    [Serializable]
    public record CannonBlockRecord
    {
        public CannonBlock.Kind[] kinds = new CannonBlock.Kind[10];
    }
    public class CannonBlock : IBlock
    {
        public Vector2Int GridPosition { get; set; }
        public int MaxPlacedBlockCount { get; } = 2;
        public CannonBlock.Kind KindValue { get; }

        public enum Kind
        {
            None,
            Kind1,
        }

        public CannonBlock(CannonBlock.Kind kind, Vector2Int gridPosition)
        {
            KindValue = kind;
            GridPosition = gridPosition;
        }

        public bool CanPickUp()
        {
            return false;
        }

        public void  PickUp(ICharacter character)
        {
            // 特になし
        }

        public bool CanPutDown(IList<IBlock> blocks)
        {
            return false;
        }
        
        public void PutDown(ICharacter character)
        {
            // 特になし
        }
    }
}