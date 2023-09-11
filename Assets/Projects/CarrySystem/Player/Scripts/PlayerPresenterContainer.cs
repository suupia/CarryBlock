using System;
using System.Collections.Generic;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

#nullable enable
namespace Carry.CarrySystem.Player.Scripts
{
            
    // AidKitのことを考えたらまとめるのが早かったかもしれない
    public class PlayerPresenterContainer : IPlayerAnimatorPresenter
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

        // IPlayerBlockPresenterで追加されるメソッド
        public void PickUpBlock(IBlock block)
        {
            _blockPresenters.ForEach(presenter => presenter.PickUpBlock(block));
        }

        public void PutDownBlock()
        {
            _blockPresenters.ForEach(presenter => presenter.PutDownBlock());
        }
        public void ReceiveBlock(IBlock block)
        {
            _blockPresenters.ForEach(presenter => presenter.ReceiveBlock(block));
        }
        
        public void PassBlock()
        {
            _blockPresenters.ForEach(presenter => presenter.PassBlock());
        }

        // IPlayerAnimatorPresenterで追加されるメソッド
        public void Idle(){
            _animatorPresenters.ForEach(presenter => presenter.Idle());
        }
        public void Walk(){
            _animatorPresenters.ForEach(presenter => presenter.Walk());
        }
        public void Dash(){
            _animatorPresenters.ForEach(presenter => presenter.Dash());
        }

    }
}