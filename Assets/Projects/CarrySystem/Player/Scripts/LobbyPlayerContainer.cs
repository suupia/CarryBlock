using System.Collections.Generic;
using System.Linq;
using Fusion;

namespace Carry.CarrySystem.Player.Scripts
{
    public class LobbyPlayerContainer
    {
        public List<LobbyPlayerControllerNet> PlayerControllers => _playerControllers.Values.ToList();
        
        Dictionary<PlayerRef,LobbyPlayerControllerNet> _playerControllers = new Dictionary<PlayerRef,LobbyPlayerControllerNet> ();
        
        public LobbyPlayerContainer()
        {
        }
        
        public void AddPlayer(PlayerRef playerRef, LobbyPlayerControllerNet playerController)
        {
            _playerControllers.Add(playerRef,playerController);
        }
    }
}