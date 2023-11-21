#nullable enable
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Spawners.Interfaces;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using UnityEngine;

namespace Carry.CarrySystem.Spawners.Scripts
{
    public class GroundPresenterLocalSpawner : IGroundPresenterSpawner

    {
    readonly IPrefabLoader<GroundPresenterLocal> _groundPresenterPrefabSpawner;

    public GroundPresenterLocalSpawner()
    {
        _groundPresenterPrefabSpawner =
            new PrefabLoaderFromAddressable<GroundPresenterLocal>("Prefabs/Map/GroundPresenterLocal");
    }

    public IGroundPresenter SpawnPrefab(Vector3 position, Quaternion rotation)
    {
        var groundPresenter = _groundPresenterPrefabSpawner.Load();
        return UnityEngine.Object.Instantiate(groundPresenter, position, rotation);
    }
    }
}