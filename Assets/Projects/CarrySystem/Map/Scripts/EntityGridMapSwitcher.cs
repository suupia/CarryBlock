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
        readonly EntityGridMapGenerator _gridMapGenerator;
        EntityGridMap _currentMap;
        
        [Inject]
        public EntityGridMapSwitcher(EntityGridMapGenerator gridMapGridMapGenerator)
        {
            _gridMapGenerator = gridMapGridMapGenerator;
            _currentMap = _gridMapGenerator.GenerateEntityGridMap(0); // indexはとりあえず0にしておく
        }
        public EntityGridMap GetMap()
        {
            return _currentMap;
        }
        
        public void NextFloor()
        {
            // Todo：　ちゃんと実装する 雰囲気はこんな感じ
            var nextMap = _gridMapGenerator.GenerateEntityGridMap(1);
            _currentMap = nextMap;
        }
    }
}