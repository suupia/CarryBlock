#nullable enable
using Carry.CarrySystem.FloorTimer.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using UnityEngine;
using Fusion;
using Carry.CarrySystem.Spawners.Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine.Serialization;
using VContainer;


namespace Carry.CarrySystem.CarryScene.Scripts
{
    public class CarryInitializerLocal : MonoBehaviour
    {    
        [SerializeField] FloorTimerLocal floorTimer;
        LocalPlayerSpawner _localPlayerSpawner;
        IMapSwitcher _entityGridMapSwitcher;
        public bool IsInitialized { get; private set; }
        
        [Inject]
        public void Construct(
            LocalPlayerSpawner networkPlayerSpawner,
            IMapSwitcher entityGridMapSwitcher
        )
        {
            _localPlayerSpawner = networkPlayerSpawner;
            _entityGridMapSwitcher = entityGridMapSwitcher;
        }
        
        void Start()
        {

            floorTimer.StartTimer();

            _entityGridMapSwitcher.InitSwitchMap();
            
            _localPlayerSpawner.RespawnAllPlayer();
            
            IsInitialized = true;
            
        }
        
    }

}
