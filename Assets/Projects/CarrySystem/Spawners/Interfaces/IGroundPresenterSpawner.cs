#nullable enable
using Carry.CarrySystem.Map.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Spawners.Interfaces
{
    public interface IGroundPresenterSpawner
    {
        public IGroundPresenter SpawnPrefab(Vector3 position, Quaternion rotation);
    }
}