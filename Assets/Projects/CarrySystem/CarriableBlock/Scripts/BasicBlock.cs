﻿#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.CarriableBlock.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using UnityEngine;

namespace Carry.CarrySystem.CarriableBlock.Scripts
{
    // JSONファイルに書き出すためにSerializableをつける
    [Serializable]
    public record BasicBlockRecord
    {
        public BasicBlock.Kind[] kinds = new BasicBlock.Kind[10];
    }

    public class BasicBlock : ICarriableBlock, IHoldable
    {
        public int MaxPlacedBlockCount { get; } = 2;
        public Kind KindValue { get; }

        public enum Kind
        {
            None,
            Kind1,
        }

        public BasicBlock(Kind kind)
        {
            KindValue = kind;
        }

        public bool CanPickUp()
        {
            return true;  // basicが持ち上げられない状況はない
        }

        public void  PickUp(IMoveExecutorSwitcher moveExecutorSwitcher, IHoldActionExecutor holdActionExecutor)
        {
            // 特になし
        }

        public bool CanPutDown(IList<ICarriableBlock> placedBlocks)
        {
            var diffList = placedBlocks.Select(x => x.GetType() != this.GetType());
            Debug.Log($"forward different block count: {diffList.Count()}, list : {string.Join(",", placedBlocks)}");
            if (placedBlocks.Count(x => x.GetType() != this.GetType()) > 0)
            {
                Debug.Log($"Basicと違う種類のBlockがあるため置けません");
                return false;
            }

            if (placedBlocks.Count >= MaxPlacedBlockCount) return false; // 現在持ち上げているBlockの設置上限数以上ならfalse

            return true;
        }
        
        public void PutDown(IMoveExecutorSwitcher moveExecutorSwitcher)
        {
           // 特になし
        }
    }
}