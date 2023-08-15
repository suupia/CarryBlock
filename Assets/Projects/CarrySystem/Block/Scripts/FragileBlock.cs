﻿using System;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Block.Scripts
{
    // JSONファイルに書き出すためにSerializableをつける
    [Serializable]
    public record FragileBlockRecord
    {
        public FragileBlock.Kind[] kinds = new FragileBlock.Kind[10];
    }

    public class FragileBlock : IBlock
    {
        public Vector2Int GridPosition { get; set; }
        public int MaxPlacedBlockCount { get; } = 1;
        public Kind KindValue { get; }

        public enum Kind
        {
            None,
            Kind1,
        }

        public FragileBlock(Kind kind, Vector2Int gridPosition)
        {
            KindValue = kind;
            GridPosition = gridPosition;
        }

        public bool CanPickUp()
        {
            return true;  // FragileBlockが持ち上げられない状況はない
        }

        public void  PickUp(ICharacter character)
        {
            
        }

        public bool CanPutDown(IList<IBlock> blocks)
        {
            var diffList = blocks.Select(x => x.GetType() != this.GetType());
            Debug.Log($"forward different block count: {diffList.Count()}, list : {string.Join(",", diffList)}");
            if (blocks.Count(x => x.GetType() != this.GetType()) > 0)
            {
                Debug.Log($"FragileBlockと違う種類のBlockがあるため置けません");
                return false;
            }

            if (blocks.Count >= MaxPlacedBlockCount) return false; // 現在持ち上げているBlockの設置上限数以上ならfalse

            return true;
        }
        
        public void PutDown(ICharacter character) 
        {
            
        }
    }
}