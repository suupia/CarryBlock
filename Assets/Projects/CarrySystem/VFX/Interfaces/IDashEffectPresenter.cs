
#nullable enable
using Carry.CarrySystem.Player.Interfaces;

namespace Carry.CarrySystem.VFX.Interfaces
{
    public interface IDashEffectPresenter
    {
        public void Init(IDashExecutor dashExecutor);
        public void StartDash();
        public void StopDash();
    }
}