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
    public class NetworkPlaceablePresenterSpawner : IPlaceablePresenterSpawner
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<PlaceablePresenterNet> _tilePresenterPrefabSpawner;

        public NetworkPlaceablePresenterSpawner(NetworkRunner runner)
        {
            _runner = runner;
            _tilePresenterPrefabSpawner =
                new PrefabLoaderFromAddressable<PlaceablePresenterNet>("Prefabs/Map/PlaceablePresenterNet");
        }

        public IPlaceablePresenter SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            var tilePresenter = _tilePresenterPrefabSpawner.Load();
            return _runner.Spawn(tilePresenter, position, rotation, PlayerRef.None);
        }
        
        // パスの時にコライダーのオン/オフを切り替える必要があるため、具象クラスを返す
        public PlaceablePresenterNet SpawnPrefabNet(Vector3 position, Quaternion rotation)
        {
            var tilePresenter = _tilePresenterPrefabSpawner.Load();
            Debug.Log("SpawnPrefabNet");
            return _runner.Spawn(tilePresenter, position, rotation, PlayerRef.None);
        }
    }
}