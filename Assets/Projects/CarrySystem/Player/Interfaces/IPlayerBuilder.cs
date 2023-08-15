using System.Diagnostics.Contracts;
using Carry.CarrySystem.Player.Scripts;
using Fusion;
using UnityEngine;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IPlayerBuilder
    {
        public AbstractNetworkPlayerController Build(Vector3 position, Quaternion rotation,
            PlayerRef playerRef);
    }
}