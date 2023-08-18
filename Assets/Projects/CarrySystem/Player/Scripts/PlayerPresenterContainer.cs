using System;
using System.Collections.Generic;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Player.Interfaces;

#nullable enable
namespace Carry.CarrySystem.Player.Scripts
{
    public class PlayerPresenterContainer : IPlayerBlockPresenter
    {
        readonly List<IPlayerBlockPresenter> _blockPresenters = new ();
        readonly List<IPlayerAnimatorPresenter> _animatorPresenters = new ();

        public PlayerPresenterContainer()
        {
        }

        public void SetHoldPresenter(IPlayerBlockPresenter presenter)
        {
            _blockPresenters.Add(presenter);

            if (presenter is IPlayerAnimatorPresenter animatorPresenter)
            {
                _animatorPresenters.Add(animatorPresenter);
            }

        }

        public void PickUpBlock(IBlock block)
        {
            _blockPresenters.ForEach(presenter => presenter.PickUpBlock(block));
        }

        public void PutDownBlock()
        {
            _blockPresenters.ForEach(presenter => presenter.PutDownBlock());
        }
        public void PassBlock()
        {
            _blockPresenters.ForEach(presenter => presenter.PassBlock());
        }

        public void ReceiveBlock(IBlock block)
        {
            _blockPresenters.ForEach(presenter => presenter.ReceiveBlock(block));
        }

    }
}