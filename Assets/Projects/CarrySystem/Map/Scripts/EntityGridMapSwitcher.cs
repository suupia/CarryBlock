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
        [Inject]
        public EntityGridMapSwitcher(EntityGridMapGenerator gridMapGridMapGenerator)
        {
            _gridMapGenerator = gridMapGridMapGenerator;
        }
        public EntityGridMap GetMap()
        {
            return   _gridMapGenerator.GenerateEntityGridMap(0); // indexはとりあえず0にしておく
        }
    }
}