using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using UnityEngine;
using Fusion;

namespace Carry.CarrySystem.Spawners.Scripts
{
    public class LocalGroundPresenterSpawner 

    {
    readonly IPrefabLoader<GroundPresenterLocal> _groundPresenterPrefabSpawner;

    public LocalGroundPresenterSpawner()
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