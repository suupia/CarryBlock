#nullable enable
using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.FloorTimer.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    /// <summary>
    ///     フロアごとに別のマップを生成し、返すクラス
    /// </summary>
    public class EntityGridMapSwitcher : IMapUpdater
    {
        readonly CartBuilder _cartBuilder;
        readonly FloorTimerNet _floorTimerNet;
        readonly EntityGridMapLoader _gridMapLoader;
        readonly TilePresenterBuilder _tilePresenterBuilder;
        int _currentIndex;
        EntityGridMap? _currentMap;

        [Inject]
        public EntityGridMapSwitcher(
            EntityGridMapLoader gridMapGridMapLoader,
            TilePresenterBuilder tilePresenterBuilder,
            CartBuilder cartBuilder,
            FloorTimerNet floorTimerNet
        )
        {
            _gridMapLoader = gridMapGridMapLoader;
            _tilePresenterBuilder = tilePresenterBuilder;
            _cartBuilder = cartBuilder;
            _floorTimerNet = floorTimerNet;
        }

        public EntityGridMap GetMap()
        {
            return _currentMap;
        }

        public void InitUpdateMap(MapKey mapKey, int index)
        {
            _currentIndex = index;
            _currentMap = _gridMapLoader.LoadEntityGridMap(mapKey, _currentIndex);
            _tilePresenterBuilder.Build(_currentMap);
            _cartBuilder.Build(_currentMap, this);
        }

        public void UpdateMap(MapKey mapKey, int index = 0)
        {
            Debug.Log($"次のフロアに変更します nextIndex: {_currentIndex + 1}");
            _currentIndex++;
            var key = MapKey.Default; // Todo: キーを決める関数を作る
            var nextMap = _gridMapLoader.LoadEntityGridMap(key, _currentIndex);
            _currentMap = nextMap;
            _tilePresenterBuilder.Build(_currentMap);


            // 以下リセット処理
            var players = Object.FindObjectsByType<CarryPlayerControllerNet>(FindObjectsSortMode.None);
            foreach (var player in players) player.Reset();

            _cartBuilder.Build(_currentMap, this);

            _floorTimerNet.StartTimer();
        }
    }
}