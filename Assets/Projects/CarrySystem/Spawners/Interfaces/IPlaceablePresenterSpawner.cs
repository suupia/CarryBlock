using Carry.CarrySystem.Map.Interfaces;
using Fusion;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Spawners.Interfaces
{
    public interface IPlaceablePresenterSpawner
    {
        public IPlaceablePresenter SpawnPrefab(Vector3 position, Quaternion rotation);
    }
}