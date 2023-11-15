using Carry.CarrySystem.Map.Scripts;
#nullable enable
namespace Carry.CarrySystem.Map.Interfaces
{
    public interface IMapSwitcher
    {
        void InitSwitchMap();
        void SwitchMap();
        void RegisterResetAction(System.Action action);
    }
}