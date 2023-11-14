#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.CarriableBlock.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using UnityEngine;

namespace Carry.CarrySystem.CarriableBlock.Scripts
{    // JSONファイルに書き出すためにSerializableをつける
    [Serializable]
    public record ConfusionBlockRecord
    {
        public ConfusionBlock.Kind[] kinds = new ConfusionBlock.Kind[10];
    }
    public class ConfusionBlock : ICarriableBlock
    {
        public Vector2Int GridPosition { get; set; }
        public int MaxPlacedBlockCount { get; } = 1;
        public Kind KindValue { get; }

        public enum Kind
        {
            None,
            Kind1,
        }

        public ConfusionBlock(Kind kind, Vector2Int gridPosition)
        {
            KindValue = kind;
            GridPosition = gridPosition;
        }

        public bool CanPickUp()
        {
            return true;  // basicが持ち上げられない状況はない
        }

        public void  PickUp(IMoveExecutorSwitcher moveExecutorSwitcher, PlayerHoldingObjectContainer blockContainer, IHoldActionExecutor holdActionExecutor)
        {
            // 上下左右を入れ替えた混乱の動きに切り替える
            moveExecutorSwitcher.SwitchToConfusionMove();
        }

        public bool CanPutDown(IList<ICarriableBlock> placedBlocks)
        {
            var diffList = placedBlocks.Select(x => x.GetType() != this.GetType());
            Debug.Log($"forward different block count: {diffList.Count()}, list : {string.Join(",", diffList)}");
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
            // 混乱状態をもとに戻す
            moveExecutorSwitcher.SwitchToRegularMove();
        }
    }
}