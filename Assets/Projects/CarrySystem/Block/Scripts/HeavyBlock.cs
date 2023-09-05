using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
#nullable  enable

namespace Carry.CarrySystem.Block.Scripts
{
    // JSONファイルに書き出すためにSerializableをつける
    [Serializable]
    public record HeavyBlockRecord
    {
        public HeavyBlock.Kind[] kinds = new HeavyBlock.Kind[10];
    }

    public class HeavyBlock : ICarriableBlock
    {
        public Vector2Int GridPosition { get; set; }
        public int MaxPlacedBlockCount { get; } = 2;
        public Kind KindValue { get; }

        public enum Kind
        {
            None,
            Kind1,
        }

        public HeavyBlock(Kind kind, Vector2Int gridPosition)
        {
            KindValue = kind;
            GridPosition = gridPosition;
        }

        public bool CanPickUp()
        {
            return true;  // basicが持ち上げられない状況はない
        }

        public void  PickUp(ICharacter character)
        {
            // 移動速度を遅くする
            character.SetSlowMoveExecutor();
        }

        public bool CanPutDown(IList<ICarriableBlock> blocks)
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
        
        public void PutDown(ICharacter character) 
        {
            // 移動速度を元に戻す
            character.SetRegularMoveExecutor();
        }
    }
}