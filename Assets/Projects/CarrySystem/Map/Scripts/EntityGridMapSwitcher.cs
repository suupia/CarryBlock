using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Fusion;
#nullable enable
namespace Carry.CarrySystem.Map.Scripts
{
    /// <summary>
    /// フロアごとに別のマップを生成し、返すクラス
    /// </summary>
    public class EntityGridMapSwitcher
    {
        readonly EntityGridMapLoader _gridMapLoader;
        readonly TilePresenterBuilder _tilePresenterBuilder;
        int _currentIndex;
        EntityGridMap _currentMap;
        
        [Inject]
        public EntityGridMapSwitcher(EntityGridMapLoader gridMapGridMapLoader, TilePresenterBuilder tilePresenterBuilder)
        {
            _gridMapLoader = gridMapGridMapLoader;
            _tilePresenterBuilder = tilePresenterBuilder;
            _currentIndex = 1; //Floor1から始まる
            var key = MapKey.Default; // Todo: キーを決める関数を作る
            _currentMap = _gridMapLoader.LoadEntityGridMap(key,_currentIndex); // indexはとりあえず0にしておく
        }
        
        public EntityGridMap GetMap()
        {
            return _currentMap;
        }
        
        public void NextFloor()
        {
            Debug.Log($"次のフロアに変更します nextIndex: {_currentIndex + 1}");
            _currentIndex++;
            var key = MapKey.Default; // Todo: キーを決める関数を作る
            var nextMap = _gridMapLoader.LoadEntityGridMap(key,_currentIndex);
            _currentMap = nextMap;
            _tilePresenterBuilder.Build(_currentMap);
            
            // 以下リセット処理
            var players = GameObject.FindObjectsByType<CarryPlayerController_Net>(FindObjectsSortMode.None);
            foreach (var player in players)
            {
                player.Reset();
            }

        }
    }
}