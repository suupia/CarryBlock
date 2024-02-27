#nullable enable
using Carry.CarrySystem.FloorTimer.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using UnityEngine;
using Fusion;
using Carry.CarrySystem.Spawners.Scripts;
using Cysharp.Threading.Tasks;
using VContainer;


namespace Carry.CarrySystem.CarryScene.Scripts
{
    public class CarryInitializerLocal : MonoBehaviour
    {    
        [SerializeField] FloorTimerNet floorTimerNet;
        NetworkPlayerSpawner _networkPlayerSpawner;
        IMapSwitcher _entityGridMapSwitcher;
        public bool IsInitialized { get; private set; }
        
        [Inject]
        public void Construct(
            NetworkPlayerSpawner networkPlayerSpawner,
            IMapSwitcher entityGridMapSwitcher
        )
        {
            _networkPlayerSpawner = networkPlayerSpawner;
            _entityGridMapSwitcher = entityGridMapSwitcher;
        }
        
        void Start()
        {

            floorTimerNet.StartTimer();

            _entityGridMapSwitcher.InitSwitchMap();
            
            _networkPlayerSpawner.RespawnAllPlayer();
            
            IsInitialized = true;
            
        }
        
    }

}
