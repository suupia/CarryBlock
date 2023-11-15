using Carry.CarrySystem.Map.Scripts;
#nullable enable
namespace Carry.CarrySystem.Map.Interfaces
{
    public interface IMapSwitcher
    {
        EntityGridMap GetMap();
        int Index { get; }
        void InitUpdateMap(MapKey mapKey, int index);
        void UpdateMap(MapKey mapKey, int index = 0);
        
        void RegisterResetAction(System.Action action);
    }
}