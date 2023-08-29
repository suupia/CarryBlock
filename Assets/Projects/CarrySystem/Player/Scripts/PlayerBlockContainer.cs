using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
#nullable  enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class PlayerBlockContainer
    {
        public bool IsHoldingBlock => _isHoldingBlock;
        public bool CanPutDown(IList<IBlockMonoDelegate> blocks) => _holdingBlock?.Block.CanPutDown(blocks.Select(b => b.Block).ToArray()) ?? false;

        bool _isHoldingBlock = false;  // 持っているかどうかの判定はIsHoldingBlockを使って外部にブロックを直接公開はしない
        IBlockMonoDelegate? _holdingBlock = null;  // 外部から取得するときは、PopBlock()を使う。
        
        /// <summary>
        /// Blockを取り出すと同時に、持っているブロックをnullにする
        /// </summary>
        /// <returns></returns>
        public IBlockMonoDelegate? PopBlock()
        {
            if (_holdingBlock == null) return null;
            var block = _holdingBlock;
            _isHoldingBlock = false;
            _holdingBlock = null;
            return block;
        }

        public void SetBlock(IBlockMonoDelegate block)
        {
            //  _presenter?.HoldBlock(block);
            _isHoldingBlock = true;
            _holdingBlock = block;
        }
        
        
    }
}