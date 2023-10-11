using Carry.CarrySystem.Map.Scripts;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Spawners.Interfaces
{
    public interface IWallPresenterLocalSpawner
    {
        public WallPresenterLocal SpawnPrefab(Vector3 position, Quaternion rotation);
    }
}