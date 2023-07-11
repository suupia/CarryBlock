using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using UnityEngine;
using Carry.CarrySystem.Player.Info;

#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class Character : ICharacter
    {
        readonly ICharacterHoldAction _holdAction;
        readonly ICharacterMove _move;
        
        public Character(ICharacterMove move, ICharacterHoldAction holdAction)
        {
            _move = move;
            _holdAction = holdAction;
        }

        public void Reset()
        {
            _holdAction.Reset();
        }

        public void Setup(PlayerInfo info)
        {
            _move.Setup(info);
            _holdAction.Setup(info);
            info.playerRb.useGravity = true;
        }

        public void Move(Vector3 direction)
        {
            _move.Move(direction);
        }
        
        public void SetHoldPresenter(IHoldActionPresenter presenter)
        {
            _holdAction.SetHoldPresenter(presenter);
        }

        public void Action()
        {
            _holdAction.Action();
        }
    }
}