#nullable enable
using UnityEngine;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IMoveFunction
    {
        public void Move(Vector3 input);
        public IMoveFunction Chain(IMoveFunction nextMoveFunction);
    }
}