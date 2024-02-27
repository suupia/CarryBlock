#nullable enable
using System.Collections.Generic;
using Carry.CarrySystem.Map.Scripts;

namespace Carry.CarrySystem.Map.Interfaces
{
    public interface IMapKeyDataSelector
    {
        public IReadOnlyList<MapKeyData> SelectMapKeyDataList(int index);
    }
}