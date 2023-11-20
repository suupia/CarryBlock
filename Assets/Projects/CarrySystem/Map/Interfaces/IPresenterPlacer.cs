using Carry.CarrySystem.Map.Scripts;
#nullable enable
namespace Carry.CarrySystem.Map.Interfaces
{
    public interface IPresenterPlacer
    {
        // InitでPlace()を一回だけ呼ぶようにして、
        // 消す処理も公開するようにするインターフェースも作成する処理を作るのがよい
        void Place(EntityGridMap map);
    }
}