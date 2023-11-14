using System.Diagnostics.Contracts;
using Carry.CarrySystem.Player.Scripts;
using Fusion;
using UnityEngine;

namespace Carry.CarrySystem.Player.Interfaces
{
    /// <summary>
    /// プレハブとドメイン（MonoDelegate, IMapUpdaterなど)を組み合わせて、CharacterControllerを生成する
    /// </summary>
    public interface IPlayerControllerNetBuilder
    {
        public AbstractNetworkPlayerController Build(Vector3 position, Quaternion rotation,
            PlayerRef playerRef);
    }
}