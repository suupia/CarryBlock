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
        CarryInitializersReady? _carryInitializersReady;
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
        
        async void Start()
        {
            _carryInitializersReady = FindObjectOfType<CarryInitializersReady>();
            if (_carryInitializersReady == null)
            {
                Debug.LogError($"_carryInitializersReady is null");
                return;
            }
            await UniTask.WaitUntil(() => _carryInitializersReady.IsAllInitializersReady());
            
            floorTimerNet.StartTimer();

            _entityGridMapSwitcher.InitSwitchMap();
            
            _networkPlayerSpawner.RespawnAllPlayer();
            
            IsInitialized = true;
            
        }
        
    }

}
