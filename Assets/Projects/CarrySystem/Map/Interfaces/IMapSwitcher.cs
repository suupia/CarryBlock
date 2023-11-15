using Carry.CarrySystem.Map.Scripts;
#nullable enable
namespace Carry.CarrySystem.Map.Interfaces
{
    public interface IMapSwitcher
    {
        EntityGridMap GetMap();
        int Index { get; }
        void InitUpdateMap();
        void UpdateMap();
        
        void RegisterResetAction(System.Action action);
    }
}