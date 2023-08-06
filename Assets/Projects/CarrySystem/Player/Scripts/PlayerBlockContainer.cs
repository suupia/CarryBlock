using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using Projects.CarrySystem.Block.Interfaces;
#nullable  enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class PlayerBlockContainer
    {
        public bool IsHoldingBlock { get; set; }
        public IBlock? HoldingBlock { get; set; }
        public IPlayerBlockPresenter Presenter => _presenter;
        
        
        readonly IMapUpdater _mapUpdater;
        IPlayerBlockPresenter? _presenter;
        bool _isHoldingBlock = false;

        public PlayerBlockContainer()
        {
            
        }

        public void SetHoldPresenter(IPlayerBlockPresenter presenter)
        {
            _presenter = presenter;
        }
    }
}