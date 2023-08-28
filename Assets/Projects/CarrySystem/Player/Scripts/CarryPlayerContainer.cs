using System.Collections.Generic;
using System.Linq;
using Fusion;

namespace Carry.CarrySystem.Player.Scripts
{
    public class CarryPlayerContainer
    {
        public List<CarryPlayerControllerNet> PlayerControllers => _playerControllers.Values.ToList();
        
        Dictionary<PlayerRef,CarryPlayerControllerNet> _playerControllers = new Dictionary<PlayerRef,CarryPlayerControllerNet> ();
        
        public CarryPlayerContainer()
        {
        }
        
        public void AddPlayer(PlayerRef playerRef, CarryPlayerControllerNet playerController)
        {
            _playerControllers.Add(playerRef,playerController);
        }
    }
}