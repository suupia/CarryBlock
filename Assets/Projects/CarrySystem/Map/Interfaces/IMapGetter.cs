
#nullable enable
using Carry.CarrySystem.Map.Scripts;

namespace Carry.CarrySystem.Map.Interfaces
{
    public interface IMapGetter
    {
        EntityGridMap GetMap();
        int Index { get; }
    }
}