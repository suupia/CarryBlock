using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;
#nullable enable
namespace Carry.CarrySystem.Map.Scripts
{
    /// <summary>
    /// フロアごとに別のマップを生成し、返すクラス
    /// </summary>
    public class EntityGridMapSwitcher
    {
        readonly EntityGridMapLoader _gridMapLoader;
        int _currentIndex;
        EntityGridMap _currentMap;
        
        [Inject]
        public EntityGridMapSwitcher(EntityGridMapLoader gridMapGridMapLoader)
        {
            _gridMapLoader = gridMapGridMapLoader;
            _currentIndex = 1; //Floor1から始まる
            _currentMap = _gridMapLoader.LoadEntityGridMap(_currentIndex); // indexはとりあえず0にしておく
        }
        public EntityGridMap GetMap()
        {
            return _currentMap;
        }
        
        public void NextFloor()
        {
            Debug.Log($"次のフロアに変更します nextIndex: {_currentIndex + 1}");
            _currentIndex++;
            var nextMap = _gridMapLoader.LoadEntityGridMap(_currentIndex);
            _currentMap = nextMap;
        }
    }
}