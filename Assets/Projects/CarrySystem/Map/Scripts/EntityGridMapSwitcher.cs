using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;
#nullable enable
namespace Carry.CarrySystem.Map.Scripts
{
    public class EntityGridMapSwitcher
    {
        public EntityGridMap GetMap()
        {
            // Todo: しっかり実装する
            return new EntityGridMap(1,0);
        }
    }
}