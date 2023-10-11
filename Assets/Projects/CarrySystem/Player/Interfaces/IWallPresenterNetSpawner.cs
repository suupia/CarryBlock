using Carry.CarrySystem.Map.Scripts;
using UnityEngine;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IWallPresenterNetSpawner
    {
        public WallPresenterNet SpawnPrefab(Vector3 position, Quaternion rotation);
    }
}