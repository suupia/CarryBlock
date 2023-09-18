using Carry.CarrySystem.Map.Scripts;

namespace Carry.CarrySystem.Map.Interfaces
{
    public interface IEntityGridMapBuilder
    {
        EntityGridMap BuildEntityGridMap(EntityGridMapData gridMapData);
    }
}