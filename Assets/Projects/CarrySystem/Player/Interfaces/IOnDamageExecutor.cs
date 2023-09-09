#nullable enable

using Carry.CarrySystem.Player.Info;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IOnDamageExecutor
    {
        public void Setup(PlayerInfo info);
        public void OnDamage();
        
    }
}