using Carry.CarrySystem.Map.Scripts;

namespace Carry.CarrySystem.Map.Interfaces
{
    /// <summary>
    /// ドメインのEntityGridMapとPresenterをつなぐ役割を持つ
    /// </summary>
    public interface IMapUpdater
    {
        EntityGridMap GetMap();
        int Index { get; }
        void InitUpdateMap(MapKey mapKey, int index);
        void UpdateMap(MapKey mapKey, int index = 0);
        
        void RegisterResetAction(System.Action action);
    }
}