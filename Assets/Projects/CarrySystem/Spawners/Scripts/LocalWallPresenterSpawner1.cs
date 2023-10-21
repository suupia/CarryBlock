using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Spawners.Interfaces;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Spawners.Scripts
{
    public class LocalWallPresenterSpawner1: IWallPresenterLocalSpawner

    {
    readonly IPrefabLoader<WallPresenterLocal> _tilePresenterPrefabSpawner;

    public LocalWallPresenterSpawner1()
    {
        _tilePresenterPrefabSpawner =
            new PrefabLoaderFromAddressable<WallPresenterLocal>("Prefabs/Map/WallPresenterLocalA_1");
    }

    public WallPresenterLocal SpawnPrefab(Vector3 position, Quaternion rotation)
    {
        var tilePresenter = _tilePresenterPrefabSpawner.Load();
        return UnityEngine.Object.Instantiate(tilePresenter, position, rotation);
    }
    }
}