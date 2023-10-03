using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.CarriableBlock.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

#nullable  enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class PlayerHoldingObjectContainer
    {
        public bool IsHoldingBlock => _holdingBlock != null;
        public bool IsHoldingAidKit => _holdingAidKit;
        public bool CanPutDown(IList<ICarriableBlock> blocks) => _holdingBlock?.CanPutDown(blocks) ?? false;

        ICarriableBlock? _holdingBlock = null;  // 外部から取得するときは、PopBlock()を使う。
        bool _holdingAidKit = false;  // AidKitに相当するドメインの処理がないためとりあえずboolで実装
        
        /// <summary>
        /// Blockを取り出すと同時に、持っているブロックをnullにする
        /// </summary>
        /// <returns></returns>
        public ICarriableBlock? PopBlock()
        {
            if (_holdingBlock == null) return null;
            var block = _holdingBlock;
            _holdingBlock = null;
            return block;
        }

        public void SetBlock(ICarriableBlock block)
        {
            //  _presenter?.HoldBlock(block);
            _holdingBlock = block;
        }
        
        public void PopAidKit()
        {
            Debug.Log($"PopAidKit");
            _holdingAidKit = false;
        }
        
        public void SetAidKit()
        {
            Debug.Log($"SetAidKit");
            // _presenter?.HoldAidKit();
            _holdingAidKit = true;
        }
        
    }
}