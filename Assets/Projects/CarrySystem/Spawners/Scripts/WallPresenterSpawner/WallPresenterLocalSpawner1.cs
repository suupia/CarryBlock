#nullable enable
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Spawners.Interfaces;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using UnityEngine;

namespace Carry.CarrySystem.Spawners.Scripts
{
    public class WallPresenterLocalSpawner1 : IWallPresenterSpawner

    {
        readonly IPrefabLoader<WallPresenterLocal> _tilePresenterPrefabSpawner;

        public WallPresenterLocalSpawner1(WallType type = WallType.A)
        {
            _tilePresenterPrefabSpawner =
                new PrefabLoaderFromAddressable<WallPresenterLocal>(
                    $"Prefabs/Map/WallPresenterLocal{type.ToString()}_1");
        }

        public IWallPresenter SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            var tilePresenter = _tilePresenterPrefabSpawner.Load();
            return UnityEngine.Object.Instantiate(tilePresenter, position, rotation);
        }
    }
}