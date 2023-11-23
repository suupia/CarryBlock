using Carry.CarrySystem.Player.Scripts;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IPlayerController
    {
        public GameObject GameObjectValue { get; }
        public Rigidbody RigidbodyValue { get; }
        public PlayerHoldingObjectContainer GetPlayerHoldingObjectContainer { get; }
        public IMoveExecutorSwitcherNew GetMoveExecutorSwitcher { get; }
        public IHoldActionExecutor GetHoldActionExecutor { get; }
    }
}