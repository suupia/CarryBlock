using Carry.CarrySystem.Map.Scripts;
#nullable enable

namespace Carry.CarrySystem.Map.Interfaces
{
    public interface IMapGetter
    {
        EntityGridMap GetMap();
        int Index { get; }
    }
}