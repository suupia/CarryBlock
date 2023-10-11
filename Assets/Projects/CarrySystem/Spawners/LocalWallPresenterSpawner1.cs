using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Spawners
{
    public class LocalWallPresenterSpawner1: IWallPresenterMonoSpawner

    {
    readonly IPrefabLoader<WallPresenterMono> _tilePresenterPrefabSpawner;

    public LocalWallPresenterSpawner1()
    {
        _tilePresenterPrefabSpawner =
            new PrefabLoaderFromAddressable<WallPresenterMono>("Prefabs/Map/WallPresenterMono1");
    }

    public WallPresenterMono SpawnPrefab(Vector3 position, Quaternion rotation)
    {
        var tilePresenter = _tilePresenterPrefabSpawner.Load();
        return UnityEngine.Object.Instantiate(tilePresenter, position, rotation);
    }
    }
}