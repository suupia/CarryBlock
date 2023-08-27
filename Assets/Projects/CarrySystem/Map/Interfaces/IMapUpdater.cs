using Carry.CarrySystem.Map.Scripts;

namespace Carry.CarrySystem.Map.Interfaces
{
    /// <summary>
    /// ドメインのEntityGridMapとPresenterをつなぐ役割を持つ
    /// </summary>
    public interface IMapUpdater
    {
        EntityGridMap GetMap();
        void InitUpdateMap(MapKey mapKey, int index); // ToDo: Initの方は引数なしかデフォルト引数があったほうがいいかも
        void UpdateMap(MapKey mapKey, int index = 0);
        
        void RegisterResetAction(System.Action action);
    }
}