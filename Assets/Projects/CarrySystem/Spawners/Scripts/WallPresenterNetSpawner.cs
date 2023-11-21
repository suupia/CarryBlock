#nullable enable
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Spawners.Interfaces;
using Fusion;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using UnityEngine;

namespace Carry.CarrySystem.Spawners.Scripts
{
    public class WallPresenterNetSpawner: IWallPresenterSpawner
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<WallPresenterNet> _tilePresenterPrefabSpawner;

        public WallPresenterNetSpawner(NetworkRunner runner)
        {
            _runner = runner;
            _tilePresenterPrefabSpawner =
                new PrefabLoaderFromAddressable<WallPresenterNet>("Prefabs/Map/WallPresenterNet");
        }
        
        public IWallPresenter SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            var tilePresenter = _tilePresenterPrefabSpawner.Load();
            return _runner.Spawn(tilePresenter, position, rotation, PlayerRef.None);
        }
    }
}