using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Spawners.Interfaces;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Spawners.Scripts
{
    public enum WallType
    {
        A,B,C
    }
    public class LocalWallPresenterSpawner : IWallPresenterLocalSpawner
    {
        readonly IPrefabLoader<WallPresenterLocal> _tilePresenterPrefabSpawner;

        public LocalWallPresenterSpawner(WallType type = WallType.A)
        {
            _tilePresenterPrefabSpawner =
                new PrefabLoaderFromAddressable<WallPresenterLocal>($"Prefabs/Map/WallPresenterLocal{type.ToString()}_0");
        }

        public WallPresenterLocal SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            var tilePresenter = _tilePresenterPrefabSpawner.Load();
            return UnityEngine.Object.Instantiate(tilePresenter, position, rotation);
        }
    }
}