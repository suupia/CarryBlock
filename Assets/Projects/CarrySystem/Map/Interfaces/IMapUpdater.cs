using Carry.CarrySystem.Map.Scripts;

namespace Carry.CarrySystem.Map.Interfaces
{
    public interface IMapUpdater
    {
        EntityGridMap GetMap();
        void InitUpdateMap(MapKey mapKey, int index); // ToDo: Initの方は引数なしかデフォルト引数があったほうがいいかも
        void UpdateMap(MapKey mapKey, int index);
    }
}