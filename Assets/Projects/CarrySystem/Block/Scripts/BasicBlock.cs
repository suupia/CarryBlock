#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Projects.CarrySystem.Block.Scripts;
using UnityEngine;

namespace Carry.CarrySystem.Block.Scripts
{
    // JSONファイルに書き出すためにSerializableをつける
    [Serializable]
    public record BasicBlockRecord
    {
        public BasicBlock.Kind[] kinds = new BasicBlock.Kind[10];
    }

    public class BasicBlock : AbstractBlock
    {
        public override int MaxPlacedBlockCount { get; } = 2;
        public Kind KindValue { get; }

        public enum Kind
        {
            None,
            Kind1,
        }

        public BasicBlock(Kind kind, Vector2Int gridPosition)
        {
            KindValue = kind;
            GridPosition = gridPosition;
        }

        public override bool CanPickUp()
        {
            return true;  // basicが持ち上げられない状況はない
        }

        public override void  PickUp(ICharacter character)
        {
            // 特になし
        }

        public override bool CanPutDown(IList<IBlock> blocks)
        {
            var diffList = blocks.Select(x => x.GetType() != this.GetType());
            Debug.Log($"forward different block count: {diffList.Count()}, list : {string.Join(",", diffList)}");
            if (blocks.Count(x => x.GetType() != this.GetType()) > 0)
            {
                Debug.Log($"Basicと違う種類のBlockがあるため置けません");
                return false;
            }

            if (blocks.Count >= MaxPlacedBlockCount) return false; // 現在持ち上げているBlockの設置上限数以上ならfalse

            return true;
        }
        
        public override void PutDown(ICharacter character)
        {
           // 特になし
        }
    }
}