using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Spawners.Interfaces;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using Fusion;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Spawners.Scripts
{
    public class NetworkEntityPresenterSpawner : IEntityPresenterSpawner
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<EntityPresenterNet> _tilePresenterPrefabSpawner;

        public NetworkEntityPresenterSpawner(NetworkRunner runner)
        {
            _runner = runner;
            _tilePresenterPrefabSpawner =
                new PrefabLoaderFromAddressable<EntityPresenterNet>("Prefabs/Map/EntityPresenter");
        }

        public IEntityPresenter SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            var tilePresenter = _tilePresenterPrefabSpawner.Load();
            return _runner.Spawn(tilePresenter, position, rotation, PlayerRef.None);
        }
        
        // パスの時にコライダーのオン/オフを切り替える必要があるため、具象クラスを返す
        public EntityPresenterNet SpawnPrefabNet(Vector3 position, Quaternion rotation)
        {
            var tilePresenter = _tilePresenterPrefabSpawner.Load();
            Debug.Log("SpawnPrefabNet");
            return _runner.Spawn(tilePresenter, position, rotation, PlayerRef.None);
        }
    }
}