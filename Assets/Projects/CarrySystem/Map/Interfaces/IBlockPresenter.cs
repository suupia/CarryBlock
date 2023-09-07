using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Map.Scripts;
#nullable enable

namespace Carry.CarrySystem.Map.Interfaces
{
    public interface IBlockPresenter
    {
        public ref BlockPresenterNet.PresentData PresentDataRef { get; }
        public void SetBlockActiveData(IBlock block, int count);
    }
}