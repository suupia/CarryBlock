using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using UnityEngine;
using Fusion;
#nullable enable

namespace Carry.CarrySystem.Spawners
{
    public class LocalWallPresenterSpawner : IWallPresenterMonoSpawner
    {
        readonly IPrefabLoader<WallPresenterMono> _tilePresenterPrefabSpawner;

        public LocalWallPresenterSpawner()
        {
            _tilePresenterPrefabSpawner =
                new PrefabLoaderFromAddressable<WallPresenterMono>("Prefabs/Map/WallPresenterMono");
        }

        public WallPresenterMono SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            var tilePresenter = _tilePresenterPrefabSpawner.Load();
            return UnityEngine.Object.Instantiate(tilePresenter, position, rotation);
        }
    }
}