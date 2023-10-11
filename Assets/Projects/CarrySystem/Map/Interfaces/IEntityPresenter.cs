using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Map.Scripts;
#nullable enable

namespace Carry.CarrySystem.Map.Interfaces
{
    public interface IEntityPresenter
    {
        public void SetEntityActiveData(IEntity entity, int count);
        public void DestroyPresenter();
    }
}