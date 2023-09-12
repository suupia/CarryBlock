using Carry.CarrySystem.Block.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IPlayerAnimatorPresenter
    {
        void PickUpBlock(IBlock block);
        void PutDownBlock();
        void ReceiveBlock(IBlock block);
        void PassBlock();
        void Idle();
        void Walk();
        void Dash();
    }
}