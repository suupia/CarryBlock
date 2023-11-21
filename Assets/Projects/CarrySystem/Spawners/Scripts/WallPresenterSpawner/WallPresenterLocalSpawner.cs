#nullable enable
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Spawners.Interfaces;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using UnityEngine;

namespace Carry.CarrySystem.Spawners.Scripts
{
    public enum WallType
    {
        A,B,C
    }
    public class WallPresenterLocalSpawner : IWallPresenterSpawner
    {
        readonly IPrefabLoader<WallPresenterLocal> _tilePresenterPrefabSpawner;

        public WallPresenterLocalSpawner(WallType type = WallType.A)
        {
            _tilePresenterPrefabSpawner =
                new PrefabLoaderFromAddressable<WallPresenterLocal>($"Prefabs/Map/WallPresenterLocal{type.ToString()}_0");
        }

        public IPresenterMono SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            var tilePresenter = _tilePresenterPrefabSpawner.Load();
            return UnityEngine.Object.Instantiate(tilePresenter, position, rotation);
        }
    }
}