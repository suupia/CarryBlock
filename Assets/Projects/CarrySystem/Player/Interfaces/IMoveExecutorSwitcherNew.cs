#nullable enable

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IMoveExecutorSwitcherNew : IMoveExecutor
    {
        public void SwitchToDashMove();
        public void SwitchOffDashMove();
        public void SwitchToSlowMove();
        public void SwitchOffSlowMove();
        public void SwitchToConfusionMove();
        public void SwitchOffConfusionMove();
        public void SwitchToFaintedMove();
        public void SwitchOffFaintedMove();
    }
}