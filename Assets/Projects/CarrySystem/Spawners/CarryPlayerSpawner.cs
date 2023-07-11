using Carry.CarrySystem.Player.Scripts;
using Fusion;
using Nuts.Utility.Scripts;
using VContainer;

namespace Carry.CarrySystem.Spawners
{
    public class CarryPlayerSpawner : AbstractNetworkPlayerSpawner<CarryPlayerController_Net>
    {
        [Inject]
        public CarryPlayerSpawner(NetworkRunner runner, IPrefabSpawner<CarryPlayerController_Net> playerPrefabSpawner) :
            base(runner, playerPrefabSpawner)
        {
        }
    }
    
    public class CarryPlayerContainer : AbstractNetworkPlayerContainer<CarryPlayerController_Net>
    {
    }
}