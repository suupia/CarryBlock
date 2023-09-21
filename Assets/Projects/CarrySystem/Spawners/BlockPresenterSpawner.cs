using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Scripts;
using Fusion;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using UnityEngine;
using UnityEngine.Tilemaps;

#nullable enable
namespace Carry.CarrySystem.Spawners
{
    public class BlockPresenterSpawner
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<BlockPresenterNet> _blockPresenterPrefabSpawner;

        public BlockPresenterSpawner(NetworkRunner runner)
        {
            _runner = runner;
            _blockPresenterPrefabSpawner =
                new PrefabLoaderFromAddressable<BlockPresenterNet>("Prefabs/Map/BlockPresenter");
        }

        public BlockPresenterNet SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            var blockPresenter = _blockPresenterPrefabSpawner.Load();
            return _runner.Spawn(blockPresenter, position, rotation, PlayerRef.None);
        }
    }

}