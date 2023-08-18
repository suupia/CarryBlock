using System.Collections.Generic;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Player.Interfaces;

#nullable enable
namespace Carry.CarrySystem.Player.Scripts
{
    public class PlayerPresenterContainer : IPlayerBlockPresenter
    {
        readonly IMapUpdater _mapUpdater;
        readonly List<IPlayerBlockPresenter> _presenters = new ();

        public PlayerPresenterContainer()
        {
        }

        public void SetHoldPresenter(IPlayerBlockPresenter presenter)
        {
            _presenters.Add(presenter);
        }

        public void PickUpBlock(IBlock block)
        {
            _presenters.ForEach(presenter => presenter.PickUpBlock(block));
        }

        public void PutDownBlock()
        {
            _presenters.ForEach(presenter => presenter.PutDownBlock());
        }

        public void ReceiveBlock(IBlock block)
        {
            _presenters.ForEach(presenter => presenter.ReceiveBlock(block));
        }

        public void PassBlock()
        {
            _presenters.ForEach(presenter => presenter.PassBlock());
        }
    }
}