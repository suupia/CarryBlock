using Carry.CarrySystem.Map.Scripts;
#nullable enable

namespace Carry.CarrySystem.Map.Interfaces
{
    public interface IEntityGridMapBuilder
    {
        public EntityGridMap BuildEntityGridMap(EntityGridMapData gridMapData);
    }
}