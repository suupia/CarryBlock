using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Spawners.Interfaces;
using Fusion;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using UnityEngine;
#nullable enable
namespace Carry.CarrySystem.Spawners.Scripts
{
    public class WallPresenterNetSpawner1: IWallPresenterNetSpawner
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<WallPresenterNet> _tilePresenterPrefabSpawner;

        public WallPresenterNetSpawner1(NetworkRunner runner)
        {
            _runner = runner;
            _tilePresenterPrefabSpawner =
                new PrefabLoaderFromAddressable<WallPresenterNet>("Prefabs/Map/WallPresenterNet2");
        }

        public WallPresenterNet SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            var tilePresenter = _tilePresenterPrefabSpawner.Load();
            return _runner.Spawn(tilePresenter, position, rotation, PlayerRef.None);
        }
    }
}