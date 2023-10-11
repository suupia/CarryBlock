using Carry.CarrySystem.Map.Scripts;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Spawners.Interfaces
{
    public interface IWallPresenterNetSpawner
    {
        public WallPresenterNet SpawnPrefab(Vector3 position, Quaternion rotation);
    }
}