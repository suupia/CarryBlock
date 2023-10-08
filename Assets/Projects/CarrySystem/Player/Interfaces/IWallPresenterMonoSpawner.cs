using Carry.CarrySystem.Map.Scripts;
using UnityEngine;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IWallPresenterMonoSpawner
    {
        public WallPresenterMono SpawnPrefab(Vector3 position, Quaternion rotation);
    }
}