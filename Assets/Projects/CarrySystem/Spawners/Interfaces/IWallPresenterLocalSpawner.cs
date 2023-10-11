using Carry.CarrySystem.Map.Scripts;
using UnityEngine;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IWallPresenterLocalSpawner
    {
        public WallPresenterLocal SpawnPrefab(Vector3 position, Quaternion rotation);
    }
}