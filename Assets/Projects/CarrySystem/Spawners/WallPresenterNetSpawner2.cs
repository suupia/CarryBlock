using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using UnityEngine;

namespace Carry.CarrySystem.Spawners
{
    public class WallPresenterNetSpawner2: IWallPresenterNetSpawner
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<WallPresenterNet> _tilePresenterPrefabSpawner;

        public WallPresenterNetSpawner2(NetworkRunner runner)
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