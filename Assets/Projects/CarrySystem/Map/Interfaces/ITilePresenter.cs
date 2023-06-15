using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Map.Scripts;

namespace Carry.CarrySystem.Map.Interfaces
{
    public interface ITilePresenter
    {
        public ref PresentData PresentDataRef { get; }
        public void SetEntityActiveData(IEntity entity, bool isActive);
    }
}