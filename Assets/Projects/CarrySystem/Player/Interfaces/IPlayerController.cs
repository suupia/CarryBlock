using Carry.CarrySystem.Player.Scripts;
using UnityEngine;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IPlayerController
    {
        public GameObject GameObject { get; }
        public Rigidbody Rigidbody { get; }
        public PlayerHoldingObjectContainer GetPlayerHoldingObjectContainer { get; }
        public IMoveExecutorSwitcher GetMoveExecutorSwitcher { get; }
        public IHoldActionExecutor GetHoldActionExecutor { get; }
    }
}