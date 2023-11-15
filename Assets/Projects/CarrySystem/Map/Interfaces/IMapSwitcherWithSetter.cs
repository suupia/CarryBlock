#nullable enable
using Carry.CarrySystem.Map.Scripts;

namespace Carry.CarrySystem.Map.Interfaces
{
    public interface IMapSwitcherWithSetter : IMapSwitcher
    {
        public void SetMapKey(MapKey mapKey);

        public void SetIndex(int index);
    }
}