using Fusion;
#nullable enable

namespace Carry.CarrySystem.Player.Interfaces
{
    public abstract class AbstractNetworkPlayerController : NetworkBehaviour
    {
        public ICharacter Character => character;

        protected ICharacter? character;

    }
}