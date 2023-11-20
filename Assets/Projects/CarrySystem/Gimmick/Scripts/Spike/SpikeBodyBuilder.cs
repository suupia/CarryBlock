using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Gimmick.Scripts;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using Fusion;
using UnityEngine;
using VContainer;
#nullable enable

namespace Projects.CarrySystem.Gimmick.Scripts
{
    public class SpikeBodyBuilder
    {
        readonly IPrefabLoader<SpikeBodyControllerNet> _spikeBodyControllerLoader;
        NetworkRunner? _runner;

        [Inject]
        public SpikeBodyBuilder()
        {
            // I decided to not DI Loader
            _spikeBodyControllerLoader = new PrefabLoaderFromAddressable<SpikeBodyControllerNet>("Prefabs/Gimmick/Spike/SpikeBodyNet");
        }
        
        public SpikeBodyControllerNet Build(SpikeGimmick.Kind kind, Vector3 position, Quaternion rotation, PlayerRef playerRef)
        {
            _runner ??= GameObject.FindWithTag("NetworkRunner").GetComponent<NetworkRunner>();
            if(_runner == null) Debug.LogError("NetworkRunner is not found");
            
            // Load prefab
            var spikeBodyControllerNet = _spikeBodyControllerLoader.Load();
            
            // Spawn prefab
            var cannonBallControllerObj = _runner.Spawn(spikeBodyControllerNet, position, rotation, playerRef,
                (runner, networkObj) =>
                {
                    networkObj.GetComponent<SpikeBodyControllerNet>().Init(kind);
                });
            
            Debug.Log($"GridPos {position}, Build Spike");
            // Setup Info
            // ...
            
            return cannonBallControllerObj;
        }
    }
}