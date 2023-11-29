using System;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.CarriableBlock.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Carry.CarrySystem.CarriableBlock.Scripts
{
    // JSONファイルに書き出すためにSerializableをつける
    [Serializable]
    public record FragileBlockRecord
    {
        public FragileBlock.Kind[] kinds = new FragileBlock.Kind[10];
    }

    public class FragileBlock : ICarriableBlock, IHoldable
    {
        public int MaxPlacedBlockCount { get; } = 1;
        public Kind KindValue { get; }

        public enum Kind
        {
            None,
            Kind1,
        }

        public FragileBlock(Kind kind)
        {
            KindValue = kind;
        }

        public bool CanPickUp()
        {
            return true;  // FragileBlockが持ち上げられない状況はない
        }

        public void  PickUp(IMoveExecutorSwitcher moveExecutorSwitcher, IHoldActionExecutor holdActionExecutor)
        {
             BreakBlock(holdActionExecutor).Forget();
        }

        public bool CanPutDown(IList<ICarriableBlock> placedBlocks)
        {
            var diffList = placedBlocks.Select(x => x.GetType() != this.GetType());
            Debug.Log($"forward different block count: {diffList.Count()}, list : {string.Join(",", diffList)}");
            if (placedBlocks.Count(x => x.GetType() != this.GetType()) > 0)
            {
                Debug.Log($"FragileBlockと違う種類のBlockがあるため置けません");
                return false;
            }

            if (placedBlocks.Count >= MaxPlacedBlockCount) return false; // 現在持ち上げているBlockの設置上限数以上ならfalse

            return true;
        }
        
        public void PutDown(IMoveExecutorSwitcher moveExecutorSwitcher) 
        {
            
        }

        async UniTaskVoid BreakBlock(IHoldActionExecutor holdActionExecutor)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.25f));
            Debug.Log("BreakBlockを実行");
            
            holdActionExecutor.ResetHoldingBlock();
        }  
    }
}