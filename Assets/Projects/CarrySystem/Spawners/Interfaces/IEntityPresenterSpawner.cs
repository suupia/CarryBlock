using Carry.CarrySystem.Map.Interfaces;
using Fusion;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Spawners.Interfaces
{
    public interface IEntityPresenterSpawner
    {
        public IEntityPresenter SpawnPrefab(Vector3 position, Quaternion rotation);
    }
}