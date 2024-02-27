#nullable enable
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;

namespace Projects.CarrySystem.Cart.Interfaces
{
    public interface ICartBuilder
    {
        void Build(EntityGridMap map, IMapSwitcher mapSwitcher);
    }
}