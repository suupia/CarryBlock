using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using UnityEngine;
using Fusion;
#nullable enable

namespace Carry.CarrySystem.Spawners.Scripts
{
    public class LocalWallPresenterSpawner : IWallPresenterLocalSpawner
    {
        readonly IPrefabLoader<WallPresenterLocal> _tilePresenterPrefabSpawner;

        public LocalWallPresenterSpawner()
        {
            _tilePresenterPrefabSpawner =
                new PrefabLoaderFromAddressable<WallPresenterLocal>("Prefabs/Map/WallPresenterLocal");
        }

        public WallPresenterLocal SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            var tilePresenter = _tilePresenterPrefabSpawner.Load();
            return UnityEngine.Object.Instantiate(tilePresenter, position, rotation);
        }
    }
}