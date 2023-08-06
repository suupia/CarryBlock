using System.Collections.Generic;
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
        public bool CanPutDown(IList<IBlock> blocks) => _holdingBlock?.CanPutDown(blocks) ?? false;
        public IPlayerBlockPresenter Presenter => _presenter;
        
        readonly IMapUpdater _mapUpdater;
        IPlayerBlockPresenter? _presenter;
        bool _isHoldingBlock = false; // 外部から取得するときは、PopBlock()を使う。
                                      // 持っているかどうかの判定はIsHoldingBlockを使って外部にブロックをちょくせつ公開はしない
        IBlock? _holdingBlock = null;

        public PlayerBlockContainer()
        {
            
        }

        public void SetHoldPresenter(IPlayerBlockPresenter presenter)
        {
            _presenter = presenter;
        }
        
        /// <summary>
        /// Blockを取り出すと同時に、持っているブロックをnullにする
        /// </summary>
        /// <returns></returns>
        public IBlock? PopBlock()
        {
            if (_holdingBlock == null) return null;
            var block = _holdingBlock;
            _isHoldingBlock = false;
            _holdingBlock = null;
            return block;
        }

        public void SetBlock(IBlock block)
        {
            //  _presenter?.HoldBlock(block);
            _isHoldingBlock = true;
            _holdingBlock = block;
        }
        
        
    }
}