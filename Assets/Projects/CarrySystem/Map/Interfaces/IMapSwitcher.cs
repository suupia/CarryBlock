using Carry.CarrySystem.Map.Scripts;
#nullable enable
namespace Carry.CarrySystem.Map.Interfaces
{
    public interface IMapSwitcher
    {
        void InitUpdateMap();
        void UpdateMap();
        void RegisterResetAction(System.Action action);
    }
}